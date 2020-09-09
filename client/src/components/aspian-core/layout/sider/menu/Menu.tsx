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
import { useTranslation } from 'react-i18next';
import { observer } from 'mobx-react-lite';
import { CoreRootStoreContext } from '../../../../../app/stores/aspian-core/CoreRootStore';

const { SubMenu } = Menu;
const AspianMenu: FC<RouteComponentProps> = ({ location }) => {
  const { t } = useTranslation(['core_menu', 'core_common']);
  // Stores
  const coreRootStore = useContext(CoreRootStoreContext);
  const { siderStore } = coreRootStore;

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
          {t('core_common:appName')}
        </span>
      </Menu.Item>
      <Menu.Item
        key="/admin"
        icon={<DashboardOutlined className="sider__menu-icon" />}
      >
        <Link to="/admin">{t('items.dashboard.name')}</Link>
      </Menu.Item>
      <SubMenu
        key="sub1"
        icon={<PushpinOutlined className="sider__menu-icon" />}
        title={t('items.posts.name')}
      >
        <Menu.Item key="/admin/posts">
          <Link to="/admin/posts">{t('items.posts.items.all-posts')}</Link>
        </Menu.Item>
        <Menu.Item key="/admin/posts/add-new">
          <Link to="/admin/posts/add-new">{t('items.posts.items.add-new')}</Link>
        </Menu.Item>
        <Menu.Item key="4">{t('items.posts.items.categories')}</Menu.Item>
        <Menu.Item key="5">{t('items.posts.items.tags')}</Menu.Item>
      </SubMenu>
      <SubMenu
        key="sub2"
        icon={<CloudServerOutlined className="sider__menu-icon" />}
        title={t('items.media.name')}
      >
        <Menu.Item key="6">{t('items.media.items.library')}</Menu.Item>
        <Menu.Item key="7">{t('items.media.items.add-new')}</Menu.Item>
      </SubMenu>
      <SubMenu
        key="sub3"
        icon={<DiffOutlined className="sider__menu-icon" />}
        title={t('items.pages.name')}
      >
        <Menu.Item key="8">{t('items.pages.items.all-pages')}</Menu.Item>
        <Menu.Item key="9">{t('items.pages.items.add-new')}</Menu.Item>
      </SubMenu>
      <Menu.Item
        key="10"
        icon={<CommentOutlined className="sider__menu-icon" />}
      >
        {t('items.comments.name')}
      </Menu.Item>
      <SubMenu
        key="sub4"
        icon={<FormatPainterOutlined className="sider__menu-icon" />}
        title={t('items.appearance.name')}
      >
        <Menu.Item key="11">{t('items.appearance.items.customize')}</Menu.Item>
        <Menu.Item key="12">{t('items.appearance.items.menus')}</Menu.Item>
      </SubMenu>
      <SubMenu
        key="sub5"
        icon={<TeamOutlined className="sider__menu-icon" />}
        title={t('items.users.name')}
      >
        <Menu.Item key="13">{t('items.users.items.all-users')}</Menu.Item>
        <Menu.Item key="14">{t('items.users.items.add-new')}</Menu.Item>
        <Menu.Item key="15">{t('items.users.items.your-profile')}</Menu.Item>
      </SubMenu>
      <SubMenu
        key="sub6"
        icon={<ControlOutlined className="sider__menu-icon" />}
        title={t('items.settings.name')}
      >
        <Menu.Item key="16">{t('items.settings.items.general')}</Menu.Item>
        <Menu.Item key="17">{t('items.settings.items.writing')}</Menu.Item>
        <Menu.Item key="18">{t('items.settings.items.reading')}</Menu.Item>
        <Menu.Item key="19">{t('items.settings.items.discussion')}</Menu.Item>
        <Menu.Item key="20">{t('items.settings.items.media')}</Menu.Item>
        <Menu.Item key="21">{t('items.settings.items.permalinks')}</Menu.Item>
      </SubMenu>
      <SubMenu
        key="sub7"
        icon={<FundViewOutlined className="sider__menu-icon" />}
        title={t('items.reports.name')}
      >
        <Menu.Item key="13">
          {t('items.reports.items.users-activities')}
        </Menu.Item>
      </SubMenu>
    </Menu>
  );
};

export default withRouter(observer(AspianMenu));
