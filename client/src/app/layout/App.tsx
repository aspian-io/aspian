import React, { Fragment, FC, useEffect } from 'react';
import { Route, Switch } from 'react-router-dom';
// Redux
import { connect } from 'react-redux';
import { IStoreState } from '../stores/reducers/index';

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
import NotFound from '../../components/aspian-core/layout/result/NotFound';
import ServerError from '../../components/aspian-core/layout/result/ServerError';
import NetworkProblem from '../../components/aspian-core/layout/result/NetworkProblem';

import { onLayoutBreakpoint } from '../stores/actions/aspian-core/layout/sider';
import {
  DirectionActionTypeEnum,
  LanguageActionTypeEnum,
} from '../stores/actions/aspian-core/locale/types';

interface IProps {
  lang: LanguageActionTypeEnum;
  onLayoutBreakpoint: (broken: boolean, isRtl: boolean) => void;
  dir: DirectionActionTypeEnum;
}

const App: FC<IProps> = ({ lang, dir, onLayoutBreakpoint }) => {
  const { Content } = Layout;

  useEffect(() => {
    if (window.innerWidth >= 992)
      onLayoutBreakpoint(
        false,
        dir === DirectionActionTypeEnum.LTR ? false : true
      );
      i18n.changeLanguage(lang);
  }, [onLayoutBreakpoint, dir, lang]);

  if (lang === LanguageActionTypeEnum.fa) {
    document.body.style.fontFamily = 'Vazir';
  }
  

  return (
    <I18nextProvider i18n={i18n}>
      <ConfigProvider
        direction={dir === DirectionActionTypeEnum.LTR ? 'ltr' : 'rtl'}
        locale={lang === LanguageActionTypeEnum.en ? enUS : faIR}
      >
        <Layout className="aspian__layout" id="appLayout">
          <Switch>
            <Route exact path="/login" component={Login} />
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
                          <Route
                            exact
                            path="/admin/posts"
                            component={PostList}
                          />
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

const mapStateToProps = ({
  localeState,
}: IStoreState): {
  lang: LanguageActionTypeEnum;
  dir: DirectionActionTypeEnum;
} => {
  const { lang, dir } = localeState;
  return { lang, dir };
};

export default connect(mapStateToProps, { onLayoutBreakpoint })(App);
