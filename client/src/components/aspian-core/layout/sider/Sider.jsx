import React from 'react';
import { Layout } from 'antd';
import AspianMenu from './menu/Menu';

const { Sider } = Layout;

const AspianSider = ({ collapsed, onLayoutBreakpoint }) => {

  return (
    <Sider
      className='sider'
      breakpoint="lg"
      collapsedWidth="0"
      trigger={null}
      collapsible
      collapsed={collapsed}
      onBreakpoint= {onLayoutBreakpoint}
    >
      <AspianMenu />
    </Sider>
  );
};

export default AspianSider;
