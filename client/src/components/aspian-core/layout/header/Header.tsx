import React, { useContext } from 'react';
import { Layout, Row, Col, Select } from 'antd';
import { MenuUnfoldOutlined, MenuFoldOutlined } from '@ant-design/icons';
import { observer } from 'mobx-react-lite';
import SiderStore from '../../../../app/stores/aspian-core/layout/siderStore';
import LocaleStore from '../../../../app/stores/aspian-core/locale/localeStore';
import {
  DirectionActionTypeEnum,
  LanguageActionTypeEnum,
} from '../../../../app/stores/aspian-core/locale/types';

const { Header } = Layout;

const AspianHeader = () => {
  // Stores
  const siderStore = useContext(SiderStore);
  const localeStore = useContext(LocaleStore);

  return (
    <Header className="header">
      <Row>
        <Col span={12}>
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
          span={12}
          style={{
            textAlign:
              localeStore.dir === DirectionActionTypeEnum.LTR
                ? 'right'
                : 'left',
          }}
        >
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
