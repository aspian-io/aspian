import React from 'react';
import { Layout } from 'antd';
import { MenuUnfoldOutlined, MenuFoldOutlined } from '@ant-design/icons';

interface IProps {
  collapsed: boolean;
  toggle: () => void;
}

const { Header } = Layout;

const AspianHeader: React.FC<IProps> = ({ collapsed, toggle }) => {
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
