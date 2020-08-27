import React, { Fragment, useEffect, useContext } from 'react';
import { Route, Switch } from 'react-router-dom';

import { I18nextProvider } from 'react-i18next';
import i18n from '../../locales/i18n';
import enUS from 'antd/es/locale/en_US';
import faIR from 'antd/es/locale/fa_IR';
import '../../scss/aspian-core/base/_font-fa.scss';

import { Layout, ConfigProvider } from 'antd';
import 'antd/dist/antd.css';

import Dashboard from '../../components/aspian-core/dashboard/Dashboard';
import AspianHeader from '../../components/aspian-core/layout/header/Header';
import AspianBreadcrumb from '../../components/aspian-core/layout/breadcrumb/AspianBreadcrumb';
import AspianSider from '../../components/aspian-core/layout/sider/Sider';
import AspianFooter from '../../components/aspian-core/layout/footer/Footer';
import PostList from '../../components/aspian-core/post/postList/PostList';
import Login from '../../components/aspian-core/user/Login';
import Register from '../../components/aspian-core/user/Register';
import BadRequest from '../../components/aspian-core/layout/result/BadRequest';
import NotFound from '../../components/aspian-core/layout/result/NotFound';
import ServerError from '../../components/aspian-core/layout/result/ServerError';
import NetworkProblem from '../../components/aspian-core/layout/result/NetworkProblem';

import { observer } from 'mobx-react-lite';
import SiderStore from '../stores/aspian-core/layout/siderStore';
import LocaleStore from '../stores/aspian-core/locale/localeStore';
import {
  DirectionActionTypeEnum,
  LanguageActionTypeEnum,
} from '../stores/aspian-core/locale/types';

const App = () => {
  // Stores
  const siderStore = useContext(SiderStore);
  const localeStore = useContext(LocaleStore);

  const { Content } = Layout;

  useEffect(() => {
    if (window.innerWidth >= 992)
      siderStore.onLayoutBreakpoint(
        false,
        localeStore.dir === DirectionActionTypeEnum.LTR ? false : true
      );
    i18n.changeLanguage(localeStore.lang);
  }, [siderStore, siderStore.onLayoutBreakpoint, localeStore.dir, localeStore.lang]);

  if (localeStore.lang === LanguageActionTypeEnum.fa) {
    document.body.style.fontFamily = 'Vazir';
  }

  return (
    <I18nextProvider i18n={i18n}>
      <ConfigProvider
        direction={
          localeStore.dir === DirectionActionTypeEnum.LTR ? 'ltr' : 'rtl'
        }
        locale={localeStore.lang === LanguageActionTypeEnum.en ? enUS : faIR}
      >
        <Layout className="aspian__layout" id="appLayout">
          <Switch>
            <Route exact path="/login" component={Login} />
            <Route
              path={'/(.+)'}
              render={() => (
                <Fragment>
                  <AspianSider />
                  <Layout
                    className="aspian__layout--content"
                    id="contentLayout"
                  >
                    <AspianHeader />
                    <Content className="content">
                      <AspianBreadcrumb />
                      <div className="content-wrapper">
                        <Switch>
                          <Route exact path="/admin" component={Dashboard} />
                          <Route exact path="/register" component={Register} />
                          <Route
                            exact
                            path="/admin/posts"
                            component={PostList}
                          />
                          <Route path="/badrequest" component={BadRequest} />
                          <Route path="/notfound" component={NotFound} />
                          <Route path="/server-error" component={ServerError} />
                          <Route
                            path="/network-error"
                            component={NetworkProblem}
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
    </I18nextProvider>
  );
};

export default observer(App);
