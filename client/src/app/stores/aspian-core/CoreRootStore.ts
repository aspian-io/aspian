import {createContext} from 'react';
import PostStore from './post/postStore';
import LocaleStore from './locale/localeStore';
import SiderStore from './layout/siderStore';
import ResultStore from './layout/resultStore/resultStore';
import UserStore from './user/userStore';
import { configure } from 'mobx';

configure({ enforceActions: 'observed' });

export class CoreRootStore {
    agent: any;
    postStore: PostStore;
    localeStore: LocaleStore;
    siderStore: SiderStore;
    resultStore: ResultStore;
    userStore: UserStore;

    constructor() {
        this.postStore = new PostStore(this);
        this.localeStore = new LocaleStore(this);
        this.siderStore = new SiderStore(this);
        this.resultStore = new ResultStore(this);
        this.userStore = new UserStore(this);
    }
}

export const CoreRootStoreContext = createContext(new CoreRootStore());