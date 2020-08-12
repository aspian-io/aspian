import React, { useState } from 'react';
import { Route } from 'react-router-dom';
import { Layout, ConfigProvider, Breadcrumb } from 'antd';
import 'antd/dist/antd.css';

import Dashboard from './components/aspian-core/dashboard/Dashboard';
import AspianHeader from './components/aspian-core/layout/header/Header';
import AspianSider from './components/aspian-core/layout/sider/Sider';
import AspianFooter from './components/aspian-core/layout/footer/Footer';
import PostList from  './components/aspian-core/post/postList/PostList';

const App = () => {
  const { Content } = Layout;
  const [collapsed, setCollapsed] = useState(false);

  const toggle = () => {
    setCollapsed(!collapsed);
    if (collapsed) {
      document.getElementById('contentLayout').style.marginLeft = '200px';
    } else {
      document.getElementById('contentLayout').style.marginLeft = '0';
    }
  };

  const onLayoutBreakpoint = (broken) => {
    if (broken) {
      document.getElementById('contentLayout').style.marginLeft = '0';
      document.getElementById('appLayout').style.overflow = 'hidden';
      document.getElementById('contentLayout').style.minWidth = `100%`;
      setCollapsed(true);
    } else {
      document.getElementById('contentLayout').style.marginLeft = '200px';
      document.getElementById('appLayout').style.overflow = 'initial';
      document.getElementById('contentLayout').style.minWidth = `initial`;
      setCollapsed(false);
    }
  };

  return (
    // <ConfigProvider direction='rtl' locale={faIR}>
    <ConfigProvider>
      <Layout className="layout" id="appLayout">
        <AspianSider
          collapsed={collapsed}
          onLayoutBreakpoint={onLayoutBreakpoint}
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
              <Route exact path='/' component={Dashboard} />
              <Route exact path='/posts' component={PostList} />
            </div>
          </Content>
          <AspianFooter />
        </Layout>
      </Layout>
    </ConfigProvider>
  );
};

export default App;
