import React from 'react';
import { Layout, Row, Col, Select } from 'antd';
import { MenuUnfoldOutlined, MenuFoldOutlined } from '@ant-design/icons';
import { IStoreState } from '../../../../app/stores/reducers';
import { connect } from 'react-redux';
import { toggle } from '../../../../app/stores/actions/aspian-core/layout/sider';
import { handleChangeLanguage } from '../../../../app/stores/actions/aspian-core/locale/locale';
import { LanguageActionTypeEnum, DirectionActionTypeEnum } from '../../../../app/stores/actions/aspian-core/locale/types';

interface IProps {
  collapsed: boolean;
  toggle: Function;
  handleChangeLanguage: (lang: LanguageActionTypeEnum) => void;
  lang: LanguageActionTypeEnum;
  dir: DirectionActionTypeEnum;
}

const { Header } = Layout;

const AspianHeader: React.FC<IProps> = ({ collapsed, toggle, handleChangeLanguage, lang, dir }) => {
  return (
    <Header className="header">
      <Row>
        <Col span={12}>
          {dir === DirectionActionTypeEnum.LTR && React.createElement(
            collapsed ? MenuUnfoldOutlined : MenuFoldOutlined,
            {
              className: 'header--trigger',
              onClick: () => toggle(collapsed),
            }
          )}
          {dir === DirectionActionTypeEnum.RTL && React.createElement(
            collapsed ? MenuFoldOutlined : MenuUnfoldOutlined,
            {
              className: 'header--trigger',
              onClick: () => toggle(collapsed),
            }
          )}
        </Col>
        <Col span={12} style={{textAlign: dir === DirectionActionTypeEnum.LTR ? "right" : "left"}}>
            <Select defaultValue={lang} style={{margin: '0 1rem'}} onChange={handleChangeLanguage} size="small">
              <Select.Option value={LanguageActionTypeEnum.en}>En</Select.Option>
              <Select.Option value={LanguageActionTypeEnum.fa}>فا</Select.Option>
            </Select>
        </Col>
      </Row>
    </Header>
  );
};

const mapStateToProps = ({
  siderState,
  localeState
}: IStoreState): { collapsed: boolean, lang: LanguageActionTypeEnum, dir: DirectionActionTypeEnum } => {
  const { collapsed } = siderState;
  const { lang, dir } = localeState;
  return { collapsed, lang, dir };
};

export default connect(mapStateToProps, { toggle, handleChangeLanguage })(AspianHeader);
