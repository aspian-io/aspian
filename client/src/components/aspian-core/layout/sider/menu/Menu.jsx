import React from 'react';
import { Menu } from 'antd';
import {
  DashboardOutlined,
  PushpinOutlined,
  CloudServerOutlined,
  DiffOutlined,
  CommentOutlined,
  FormatPainterOutlined,
  TeamOutlined,
  ControlOutlined,
} from '@ant-design/icons';
import { Link } from 'react-router-dom';

const { SubMenu } = Menu;

const AspianMenu = () => {
  return (
    <Menu theme="dark" mode="inline" defaultSelectedKeys={['1']}>
      <Menu.Item className="menu-logo" disabled>
        <span className="anticon">
          <img
            src='/assets/Logo.svg'
            alt="logo"
            width="24px"
            height="24px"
            style={{ marginTop: '-8px' }}
          />
        </span>

        <span style={{ color: '#fff', fontSize: '1.1rem' }}>ASPIAN</span>
      </Menu.Item>
      <Menu.Item key="1" icon={<DashboardOutlined className="menu-icon" />}>
        <Link to="/">Dashboard</Link>
      </Menu.Item>
      <SubMenu
        key="sub1"
        icon={<PushpinOutlined className="menu-icon" />}
        title="Posts"
      >
        <Menu.Item key="2">
          <Link to="/posts">All Posts</Link>
        </Menu.Item>
        <Menu.Item key="3">Add New</Menu.Item>
        <Menu.Item key="4">Categories</Menu.Item>
        <Menu.Item key="5">Tags</Menu.Item>
      </SubMenu>
      <SubMenu
        key="sub2"
        icon={<CloudServerOutlined className="menu-icon" />}
        title="Media"
      >
        <Menu.Item key="6">Library</Menu.Item>
        <Menu.Item key="7">Add New</Menu.Item>
      </SubMenu>
      <SubMenu
        key="sub3"
        icon={<DiffOutlined className="menu-icon" />}
        title="Pages"
      >
        <Menu.Item key="8">All Pages</Menu.Item>
        <Menu.Item key="9">Add New</Menu.Item>
      </SubMenu>
      <Menu.Item key="10" icon={<CommentOutlined className="menu-icon" />}>
        Comments
      </Menu.Item>
      <SubMenu
        key="sub4"
        icon={<FormatPainterOutlined className="menu-icon" />}
        title="Appearance"
      >
        <Menu.Item key="11">Customize</Menu.Item>
        <Menu.Item key="12">Menus</Menu.Item>
      </SubMenu>
      <SubMenu
        key="sub5"
        icon={<TeamOutlined className="menu-icon" />}
        title="Users"
      >
        <Menu.Item key="13">All Users</Menu.Item>
        <Menu.Item key="14">Add New</Menu.Item>
        <Menu.Item key="15">Your Profile</Menu.Item>
      </SubMenu>
      <SubMenu
        key="sub6"
        icon={<ControlOutlined className="menu-icon" />}
        title="Settings"
      >
        <Menu.Item key="16">General</Menu.Item>
        <Menu.Item key="17">Writing</Menu.Item>
        <Menu.Item key="18">Reading</Menu.Item>
        <Menu.Item key="19">Discussion</Menu.Item>
        <Menu.Item key="20">Media</Menu.Item>
        <Menu.Item key="21">Permalinks</Menu.Item>
      </SubMenu>
    </Menu>
  );
};

export default AspianMenu;
