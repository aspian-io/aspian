import React, { Fragment, useEffect, useContext } from 'react';
import { Route, Switch, Redirect } from 'react-router-dom';

import i18n from '../../locales/i18n';
import enUS from 'antd/es/locale/en_US';
import faIR from 'antd/es/locale/fa_IR';
import '../../scss/aspian-core/base/_font-fa.scss';

import { Layout, ConfigProvider, Spin } from 'antd';
import 'antd/dist/antd.css';

import Dashboard from '../../components/aspian-core/dashboard/Dashboard';
import AspianHeader from '../../components/aspian-core/layout/header/Header';
import AspianBreadcrumb from '../../components/aspian-core/layout/breadcrumb/AspianBreadcrumb';
import AspianSider from '../../components/aspian-core/layout/sider/Sider';
import AspianFooter from '../../components/aspian-core/layout/footer/Footer';
import PostList from '../../components/aspian-core/post/postList/PostList';
import PostDetails from '../../components/aspian-core/post/postDetails/PostDetails';
import PostCreate from '../../components/aspian-core/post/postCreate/PostCreate';
import Login from '../../components/aspian-core/user/Login';
import Register from '../../components/aspian-core/user/Register';
import ResultPage from '../../components/aspian-core/layout/result/ResultPage';
import BadRequest from '../../components/aspian-core/layout/result/BadRequest';
import NotFound from '../../components/aspian-core/layout/result/NotFound';
import ServerError from '../../components/aspian-core/layout/result/ServerError';
import NetworkProblem from '../../components/aspian-core/layout/result/NetworkProblem';
import Unathorized401 from '../../components/aspian-core/layout/result/Unathorized401';
import Unathorized403 from '../../components/aspian-core/layout/result/Unathorized403';

import { observer } from 'mobx-react-lite';
import {
  DirectionActionTypeEnum,
  LanguageActionTypeEnum,
} from '../stores/aspian-core/locale/types';
import { CoreRootStoreContext } from '../stores/aspian-core/CoreRootStore';

const App = () => {
  // Stores
  const coreRootStore = useContext(CoreRootStoreContext);
  const { siderStore, localeStore } = coreRootStore;
  const {
    user,
    getCurrentUser,
    setAppLoaded,
    isAppLoaded,
    isLoggedIn,
  } = coreRootStore.userStore;

  const { Content } = Layout;

  useEffect(() => {
    if (user === null) {
      getCurrentUser().then(() => setAppLoaded());
    } else {
      setAppLoaded();
    }
    if (window.innerWidth >= 992)
      siderStore.onLayoutBreakpoint(
        false,
        localeStore.dir === DirectionActionTypeEnum.LTR ? false : true
      );
    i18n.changeLanguage(localeStore.lang);
  }, [
    siderStore,
    siderStore.onLayoutBreakpoint,
    localeStore.dir,
    localeStore.lang,
    getCurrentUser,
    user,
    setAppLoaded,
  ]);

  if (localeStore.lang === LanguageActionTypeEnum.fa) {
    document.body.style.fontFamily = 'Vazir';
  }

  if (!isAppLoaded) {
    return (
      <div className="spinner-wrapper">
        <Spin wrapperClassName="spinner-wrapper" />
      </div>
    );
  }

  return (
    <ConfigProvider
      direction={
        localeStore.dir === DirectionActionTypeEnum.LTR ? 'ltr' : 'rtl'
      }
      locale={localeStore.lang === LanguageActionTypeEnum.en ? enUS : faIR}
    >
      <Layout className="aspian__layout" id="appLayout">
        <Switch>
          <Route
            exact
            path="/"
            render={() =>
              isLoggedIn && user ? (
                <Redirect to="/admin" />
              ) : (
                <Redirect to="/login" />
              )
            }
          />
          {!isLoggedIn && !user && (
            <Route exact path="/login" component={Login} />
          )}
          {isLoggedIn && user && (
            <Route
              exact
              path="/login"
              render={() => <Redirect to="/admin" />}
            />
          )}
          <Route exact path="/unathorized401" component={Unathorized401} />
          <Route exact path="/unathorized403" component={Unathorized403} />
          <Route
            path={'/(.+)'}
            render={() => (
              <Fragment>
                <AspianSider />
                <Layout className="aspian__layout--content" id="contentLayout">
                  <AspianHeader />
                  <Content className="content">
                    <AspianBreadcrumb />
                    <div className="content-wrapper">
                      <Switch>
                        <Route exact path="/admin" component={Dashboard} />
                        <Route exact path="/register" component={Register} />
                        <Route exact path="/admin/posts" component={PostList} />
                        <Route
                          exact
                          path="/admin/posts/details/:id"
                          component={PostDetails}
                        />
                        <Route
                          path="/admin/posts/add-new"
                          component={PostCreate}
                        />
                        <Route path="/badrequest" component={BadRequest} />
                        <Route path="/notfound" component={NotFound} />
                        <Route path="/server-error" component={ServerError} />
                        <Route
                          path="/network-error"
                          component={NetworkProblem}
                        />
                        <Route
                          path={['/admin/post-deletion-result']}
                          component={ResultPage}
                        />
                      </Switch>
                    </div>
                  </Content>
                  <AspianFooter />
                </Layout>
              </Fragment>
            )}
          />
        </Switch>
      </Layout>
    </ConfigProvider>
  );
};

export default observer(App);
