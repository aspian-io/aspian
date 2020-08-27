import React, { FC, useContext } from 'react';
import { Menu } from 'antd';
import Logo from '../../../../../assets/Logo.svg';
import {
  DashboardOutlined,
  PushpinOutlined,
  CloudServerOutlined,
  DiffOutlined,
  CommentOutlined,
  FormatPainterOutlined,
  TeamOutlined,
  ControlOutlined,
  FundViewOutlined,
} from '@ant-design/icons';
import { Link, RouteComponentProps, withRouter } from 'react-router-dom';
import { withTranslation, WithTranslation, Trans } from 'react-i18next';
import { observer } from 'mobx-react-lite';
import SiderStore from '../../../../../app/stores/aspian-core/layout/siderStore';

type IProps = WithTranslation & RouteComponentProps;

const { SubMenu } = Menu;
const AspianMenu: FC<IProps> = ({ t, location }) => {
  // Stores
  const siderStore = useContext(SiderStore)

  return (
    <Menu
      theme="dark"
      mode="inline"
      selectedKeys={[location.pathname]}
      onSelect={({ item, key, keyPath, selectedKeys, domEvent }) =>
      siderStore.toggle(false)
      }
    >
      <Menu.Item className="sider__menu-logo" disabled>
        <span className="anticon">
          <img
            src={Logo}
            alt="logo"
            width="24px"
            height="24px"
            style={{ marginTop: '-8px' }}
          />
        </span>

        <span style={{ color: '#fff', fontSize: '1.1rem' }}>
          <Trans>{t('appName')}</Trans>
        </span>
      </Menu.Item>
      <Menu.Item
        key="/admin"
        icon={<DashboardOutlined className="sider__menu-icon" />}
      >
        <Link to="/admin">
          <Trans>{t('sider.menu.items.dashboard.name')}</Trans>
        </Link>
      </Menu.Item>
      <SubMenu
        key="sub1"
        icon={<PushpinOutlined className="sider__menu-icon" />}
        title={<Trans>{t('sider.menu.items.posts.name')}</Trans>}
      >
        <Menu.Item key="/admin/posts">
          <Link to="/admin/posts">
            <Trans>{t('sider.menu.items.posts.items.all-posts')}</Trans>
          </Link>
        </Menu.Item>
        <Menu.Item key="3">
          <Trans>{t('sider.menu.items.posts.items.add-new')}</Trans>
        </Menu.Item>
        <Menu.Item key="4">
          <Trans>{t('sider.menu.items.posts.items.categories')}</Trans>
        </Menu.Item>
        <Menu.Item key="5">
          <Trans>{t('sider.menu.items.posts.items.tags')}</Trans>
        </Menu.Item>
      </SubMenu>
      <SubMenu
        key="sub2"
        icon={<CloudServerOutlined className="sider__menu-icon" />}
        title={<Trans>{t('sider.menu.items.media.name')}</Trans>}
      >
        <Menu.Item key="6">
          <Trans>{t('sider.menu.items.media.items.library')}</Trans>
        </Menu.Item>
        <Menu.Item key="7">
          <Trans>{t('sider.menu.items.media.items.add-new')}</Trans>
        </Menu.Item>
      </SubMenu>
      <SubMenu
        key="sub3"
        icon={<DiffOutlined className="sider__menu-icon" />}
        title={<Trans>{t('sider.menu.items.pages.name')}</Trans>}
      >
        <Menu.Item key="8">
          <Trans>{t('sider.menu.items.pages.items.all-pages')}</Trans>
        </Menu.Item>
        <Menu.Item key="9">
          <Trans>{t('sider.menu.items.pages.items.add-new')}</Trans>
        </Menu.Item>
      </SubMenu>
      <Menu.Item
        key="10"
        icon={<CommentOutlined className="sider__menu-icon" />}
      >
        <Trans>{t('sider.menu.items.comments.name')}</Trans>
      </Menu.Item>
      <SubMenu
        key="sub4"
        icon={<FormatPainterOutlined className="sider__menu-icon" />}
        title={<Trans>{t('sider.menu.items.appearance.name')}</Trans>}
      >
        <Menu.Item key="11">
          <Trans>{t('sider.menu.items.appearance.items.customize')}</Trans>
        </Menu.Item>
        <Menu.Item key="12">
          <Trans>{t('sider.menu.items.appearance.items.menus')}</Trans>
        </Menu.Item>
      </SubMenu>
      <SubMenu
        key="sub5"
        icon={<TeamOutlined className="sider__menu-icon" />}
        title={<Trans>{t('sider.menu.items.users.name')}</Trans>}
      >
        <Menu.Item key="13">
          <Trans>{t('sider.menu.items.users.items.all-users')}</Trans>
        </Menu.Item>
        <Menu.Item key="14">
          <Trans>{t('sider.menu.items.users.items.add-new')}</Trans>
        </Menu.Item>
        <Menu.Item key="15">
          <Trans>{t('sider.menu.items.users.items.your-profile')}</Trans>
        </Menu.Item>
      </SubMenu>
      <SubMenu
        key="sub6"
        icon={<ControlOutlined className="sider__menu-icon" />}
        title={<Trans>{t('sider.menu.items.settings.name')}</Trans>}
      >
        <Menu.Item key="16">
          <Trans>{t('sider.menu.items.settings.items.general')}</Trans>
        </Menu.Item>
        <Menu.Item key="17">
          <Trans>{t('sider.menu.items.settings.items.writing')}</Trans>
        </Menu.Item>
        <Menu.Item key="18">
          <Trans>{t('sider.menu.items.settings.items.reading')}</Trans>
        </Menu.Item>
        <Menu.Item key="19">
          <Trans>{t('sider.menu.items.settings.items.discussion')}</Trans>
        </Menu.Item>
        <Menu.Item key="20">
          <Trans>{t('sider.menu.items.settings.items.media')}</Trans>
        </Menu.Item>
        <Menu.Item key="21">
          <Trans>{t('sider.menu.items.settings.items.permalinks')}</Trans>
        </Menu.Item>
      </SubMenu>
      <SubMenu
        key="sub7"
        icon={<FundViewOutlined className="sider__menu-icon" />}
        title={<Trans>{t('sider.menu.items.reports.name')}</Trans>}
      >
        <Menu.Item key="13">
          <Trans>{t('sider.menu.items.reports.items.users-activities')}</Trans>
        </Menu.Item>
      </SubMenu>
    </Menu>
  );
};

export default withRouter(withTranslation()(observer(AspianMenu)));
