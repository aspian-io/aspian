import { observable, computed, action, runInAction, reaction } from 'mobx';
import { IUser, IUserFormValues } from '../../../models/aspian-core/user';
import common from '../../../api/common';
import agent from '../../../api/aspian-core/agent';
import { CoreRootStore } from '../CoreRootStore';
import { history } from '../../../..';
import { message } from 'antd';

export default class UserStore {
  coreRootStore: CoreRootStore;
  constructor(coreRootStore: CoreRootStore) {
    this.coreRootStore = coreRootStore;

    reaction(
      () => this.user,
      (user) => {
        if (user === null) {
          this.getCurrentUser();
        }
      }
    );
  }

  @observable user: IUser | null = null;
  @observable submitting: boolean = false;
  @observable loginError: string = '';
  @observable isAppLoaded: boolean = false;

  @computed get isLoggedIn() {
    return !!this.user;
  }

  @action login = async (values: IUserFormValues) => {
    try {
      common.SetLogin();
      this.submitting = true;
      const user = await agent.User.login(values);
      runInAction('login action - change current logged in user state', () => {
        this.user = user;
      });
      common.SetAuthHeader(user.token);
      history.push('/admin');
    } catch (error) {
      console.log(error);
      runInAction('login action - error', () => {
        if (error.status === 401) {
          this.loginError = 'Username or password is not correct!';
        } else {
          this.loginError = `Error code: ${error.status} - ${error.statusText}`;
        }
      });
    } finally {
      runInAction('login action - submitting false - finally', () => {
        this.submitting = false;
      });
    }
  };

  @action getCurrentUser = async () => {
    try {
      const current = await agent.User.current();
      runInAction('getCurrentUser', () => {
        this.user = current;
      });
    } catch (error) {
      runInAction('getCurrentUser - error', () => {
        this.user = null;
      });
      history.push('/login');
    }
  };

  @action setAppLoaded = async () => {
    this.isAppLoaded = true;
  };

  @action logout = async () => {
    try {
      this.isAppLoaded = false;
      await agent.User.logout();
      runInAction('logout action - set user to null', () => {
        this.user = null;
      });
      common.SetAuthHeader('');
      common.SetLogout();
      history.push('/login');
    } catch (error) {
      message.error(error);
    } finally {
      runInAction('logout - finally submitting false', () => {
        this.isAppLoaded = true;
      });
    }
  };
}
