import React, { useState, Fragment, FC } from 'react';
import { Router, Route, Switch } from 'react-router-dom';
import { createBrowserHistory } from 'history';
// Redux
import { Provider } from 'react-redux';
import store from '../stores/store';

import { I18nextProvider } from 'react-i18next';
import i18n from '../../locales/i18n';
import faIR from 'antd/es/locale/fa_IR';
import '../../scss/aspian-core/base/_font-fa.scss';

import { Layout, ConfigProvider, Breadcrumb } from 'antd';
import 'antd/dist/antd.css';

import Dashboard from '../../components/aspian-core/dashboard/Dashboard';
import AspianHeader from '../../components/aspian-core/layout/header/Header';
import AspianSider from '../../components/aspian-core/layout/sider/Sider';
import AspianFooter from '../../components/aspian-core/layout/footer/Footer';
import PostList from '../../components/aspian-core/post/postList/PostList';
import Login from '../../components/aspian-core/user/Login';
import Register from '../../components/aspian-core/user/Register';
import NotFound from '../../components/aspian-core/layout/result/NotFound';
import ServerError from '../../components/aspian-core/layout/result/ServerError';
import NetworkProblem from '../../components/aspian-core/layout/result/NetworkProblem';

export const history = createBrowserHistory();

const App: FC = () => {
  const { Content } = Layout;
  const [collapsed, setCollapsed] = useState(false);
  const lang = 'fa';

  if (lang === 'fa')
  {
    document.body.style.fontFamily = "Vazir";
  }

  const toggle = () => {
    const contentLayoutDirectionIsRtl = document
      .getElementById('contentLayout')!
      .classList.contains('ant-layout-rtl');

    setCollapsed(!collapsed);
    if (collapsed) {
      if (!contentLayoutDirectionIsRtl)
        document.getElementById('contentLayout')!.style.marginLeft = '200px';
      else
        document.getElementById('contentLayout')!.style.marginRight = '200px';
    } else {
      if (!contentLayoutDirectionIsRtl)
        document.getElementById('contentLayout')!.style.marginLeft = '0';
      else document.getElementById('contentLayout')!.style.marginRight = '0';
    }
  };

  const onLayoutBreakpoint = (broken: boolean): void => {
    const contentLayoutDirectionIsRtl = document
      .getElementById('contentLayout')!
      .classList.contains('ant-layout-rtl');
    if (broken) {
      if (!contentLayoutDirectionIsRtl)
        document.getElementById('contentLayout')!.style.marginLeft = '0';
      else if (contentLayoutDirectionIsRtl)
        document.getElementById('contentLayout')!.style.marginRight = '0';

      document.getElementById('contentLayout')!.style.marginLeft = '0';
      document.getElementById('appLayout')!.style.overflow = 'hidden';
      document.getElementById('contentLayout')!.style.minWidth = `100%`;
      setCollapsed(true);
    } else {
      if (!contentLayoutDirectionIsRtl)
        document.getElementById('contentLayout')!.style.marginLeft = '200px';
      else if (contentLayoutDirectionIsRtl)
        document.getElementById('contentLayout')!.style.marginRight = '200px';

      document.getElementById('appLayout')!.style.overflow = 'initial';
      document.getElementById('contentLayout')!.style.minWidth = `initial`;
      setCollapsed(false);
    }
  };
  i18n.changeLanguage('fa');
  
  return (
    //<ConfigProvider direction="rtl" locale={faIR}>
    <Provider store={store}>
      <Router history={history}>
        <ConfigProvider direction="rtl" locale={faIR}>
          <I18nextProvider i18n={i18n}>
            <Layout className="layout" id="appLayout">
              <Switch>
                <Route exact path="/login" component={Login} />
                <Route
                  path={'/(.+)'}
                  render={() => (
                    <Fragment>
                      <AspianSider
                        collapsed={collapsed}
                        onLayoutBreakpoint={onLayoutBreakpoint}
                        pathname={history.location.pathname}
                      />
                      <Layout id="contentLayout">
                        <AspianHeader collapsed={collapsed} toggle={toggle} />
                        <Content
                          className="content"
                          style={{ margin: '24px 16px 0', overflow: 'initial' }}
                        >
                          <Breadcrumb className="breadcrumb">
                            <Breadcrumb.Item>User</Breadcrumb.Item>
                            <Breadcrumb.Item>Bill</Breadcrumb.Item>
                          </Breadcrumb>
                          <div className="content-wrapper">
                            <Switch>
                              <Route
                                exact
                                path="/admin/dashboard"
                                component={Dashboard}
                              />
                              <Route
                                exact
                                path="/register"
                                component={Register}
                              />
                              <Route
                                exact
                                path="/admin/posts"
                                component={PostList}
                              />
                              <Route path="/notfound" component={NotFound} />
                              <Route
                                path="/server-error"
                                component={ServerError}
                              />
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
          </I18nextProvider>
        </ConfigProvider>
      </Router>
    </Provider>
  );
};

export default App;
