import React from 'react';
import { Layout } from 'antd';
import { MenuUnfoldOutlined, MenuFoldOutlined } from '@ant-design/icons';

const { Header } = Layout;

const AspianHeader = ({ collapsed, toggle }) => {
  return (
    <Header
      className="header"
    >
      {React.createElement(collapsed ? MenuUnfoldOutlined : MenuFoldOutlined, {
        className: 'header--trigger',
        onClick: () => toggle(),
      })}
    </Header>
  );
};

export default AspianHeader;
