import React, { useContext } from 'react';
import { Layout, Row, Col, Select, Menu, Dropdown } from 'antd';
import {
  MenuUnfoldOutlined,
  MenuFoldOutlined,
  DownOutlined,
  ProfileOutlined,
  SettingOutlined,
  LogoutOutlined,
} from '@ant-design/icons';
import { observer } from 'mobx-react-lite';
import {
  DirectionActionTypeEnum,
  LanguageActionTypeEnum,
} from '../../../../app/stores/aspian-core/locale/types';
import { CoreRootStoreContext } from '../../../../app/stores/aspian-core/CoreRootStore';
import Avatar from 'antd/lib/avatar/avatar';
import GetRandomColor from '../../../../utils/aspian-core/base/GetRandomColor';
import agent from '../../../../app/api/aspian-core/agent';
import { useTranslation } from 'react-i18next';

const { Header } = Layout;

const AspianHeader = () => {
  const { t } = useTranslation('core_header');
  // Stores
  const coreRootStore = useContext(CoreRootStoreContext);
  const { siderStore, localeStore, userStore } = coreRootStore;

  const menu = (
    <Menu>
      <Menu.Item key="0" style={{ fontSize: '.7rem' }}>
        <a href="#!">
          <ProfileOutlined className="text primary-color" />{' '}
          {t('user-menu.items.view-profile')}
        </a>
      </Menu.Item>
      <Menu.Item key="1" style={{ fontSize: '.7rem' }}>
        <a href="#!">
          <SettingOutlined className="text primary-color" />{' '}
          {t('user-menu.items.account-settings')}
        </a>
      </Menu.Item>
      <Menu.Divider />
      <Menu.Item
        key="3"
        style={{ fontSize: '.7rem' }}
        onClick={() => userStore.logout()}
      >
        <LogoutOutlined className="text danger-color" />
        {t('user-menu.items.logout')}
      </Menu.Item>
    </Menu>
  );

  return (
    <Header className="header">
      <Row>
        <Col span={4}>
          {localeStore.dir === DirectionActionTypeEnum.LTR &&
            React.createElement(
              siderStore.collapsed ? MenuUnfoldOutlined : MenuFoldOutlined,
              {
                className: 'header--trigger',
                onClick: () => siderStore.toggle(siderStore.collapsed),
              }
            )}
          {localeStore.dir === DirectionActionTypeEnum.RTL &&
            React.createElement(
              siderStore.collapsed ? MenuFoldOutlined : MenuUnfoldOutlined,
              {
                className: 'header--trigger',
                onClick: () => siderStore.toggle(siderStore.collapsed),
              }
            )}
        </Col>
        <Col
          span={20}
          style={{
            textAlign:
              localeStore.dir === DirectionActionTypeEnum.LTR
                ? 'right'
                : 'left',
          }}
        >
          <Dropdown overlay={menu} trigger={['click']}>
            <a
              href="#!"
              style={{ padding: '1.3rem 0' }}
              onClick={(e) => e.preventDefault()}
            >
              <Avatar
                className="header__profile-photo"
                style={{
                  backgroundColor: GetRandomColor(),
                  verticalAlign: 'middle',
                }}
                src={
                  userStore.user?.profilePhotoName
                    ? agent.Attachments.getFileUrl(
                        userStore.user!.profilePhotoName
                      )
                    : ''
                }
              >
                {userStore.user?.displayName}
              </Avatar>
              <span className="header__avatar-text">
                {t('user-menu.user-greeting')} {userStore.user?.displayName}
              </span>{' '}
              <DownOutlined />
            </a>
          </Dropdown>
          <Select
            defaultValue={localeStore.lang}
            style={{ margin: '0 1rem' }}
            onChange={localeStore.handleChangeLanguage}
            size="small"
          >
            <Select.Option value={LanguageActionTypeEnum.en}>En</Select.Option>
            <Select.Option value={LanguageActionTypeEnum.fa}>فا</Select.Option>
          </Select>
        </Col>
      </Row>
    </Header>
  );
};

export default observer(AspianHeader);
