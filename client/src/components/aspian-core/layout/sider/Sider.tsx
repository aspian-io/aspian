import React from 'react';
import { Layout } from 'antd';
import AspianMenu from './menu/Menu';
import { IStoreState } from '../../../../app/stores/reducers';
import { connect } from 'react-redux';
import {onLayoutBreakpoint} from '../../../../app/stores/actions/aspian-core/layout/sider';
import { LanguageActionTypeEnum } from '../../../../app/stores/actions/aspian-core/locale/types';

interface IProps {
  collapsed: boolean;
  onLayoutBreakpoint: (broken: boolean, isRtl: boolean) => void;
  lang: LanguageActionTypeEnum
}

const { Sider } = Layout;

const AspianSider: React.FC<IProps> = ({collapsed, onLayoutBreakpoint, lang}) => {
  return (
    <Sider
      className='sider'
      breakpoint="lg"
      collapsedWidth="0"
      trigger={null}
      collapsible
      collapsed={collapsed}
      onBreakpoint= {(broken) => onLayoutBreakpoint(broken, lang === LanguageActionTypeEnum.en ? false : true)}
    >
      <AspianMenu />
    </Sider>
  );
};

const mapStateToProps = ({siderState, localeState}: IStoreState): {collapsed: boolean, lang: LanguageActionTypeEnum} => {
  const  {collapsed} = siderState;
  const  {lang} = localeState;
  return {collapsed, lang};
}

export default connect(mapStateToProps, {onLayoutBreakpoint})(AspianSider);
