import React from 'react';
import { Layout, Row, Col, Select } from 'antd';
import { MenuUnfoldOutlined, MenuFoldOutlined } from '@ant-design/icons';
import { IStoreState } from '../../../../app/stores/reducers';
import { connect } from 'react-redux';
import { toggle } from '../../../../app/stores/actions/aspian-core/layout/sider';
import { handleChangeLang } from '../../../../app/stores/actions/aspian-core/layout/header';

interface IProps {
  collapsed: boolean;
  toggle: Function;
  handleChangeLang: (value: string) => void;
  lang: string;
}

const { Header } = Layout;

const AspianHeader: React.FC<IProps> = ({ collapsed, toggle, handleChangeLang, lang }) => {
  // const handleChange = (value: LanguageEnum) => {
  //   localStorage.setItem("aspianCmsLang", value);
  // }
  //const lang = localStorage.getItem("aspianCmsLang");
  return (
    <Header className="header">
      <Row>
        <Col span={12}>
          {React.createElement(
            collapsed ? MenuUnfoldOutlined : MenuFoldOutlined,
            {
              className: 'header--trigger',
              onClick: () => toggle(collapsed),
            }
          )}
        </Col>
        <Col span={12} style={{textAlign: lang === "en" || null ? "right" : "left"}}>
            <Select defaultValue={lang ?? "en"} style={{margin: '0 1rem'}} onChange={handleChangeLang} size="small">
              <Select.Option value="en">En</Select.Option>
              <Select.Option value="fa">فا</Select.Option>
            </Select>
        </Col>
      </Row>
    </Header>
  );
};

const mapStateToProps = ({
  siderState,
  headerState
}: IStoreState): { collapsed: boolean, lang: string } => {
  const { collapsed } = siderState;
  const { lang } = headerState;
  return { collapsed, lang };
};

export default connect(mapStateToProps, { toggle, handleChangeLang })(AspianHeader);
