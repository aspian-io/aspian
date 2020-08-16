import React from 'react';
import { Layout } from 'antd';
import AspianMenu from './menu/Menu';
import { IStoreState } from '../../../../app/stores/reducers';
import { connect } from 'react-redux';
import {onLayoutBreakpoint} from '../../../../app/stores/actions/aspian-core/layout/sider';
import { withRouter } from 'react-router-dom';

interface IProps {
  collapsed: boolean;
  onLayoutBreakpoint: (broken: boolean, isRtl: boolean) => void;
  lang: string
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
      onBreakpoint= {(broken) => onLayoutBreakpoint(broken, lang === 'en' ? false : true)}
    >
      <AspianMenu />
    </Sider>
  );
};

const mapStateToProps = ({siderState, headerState}: IStoreState): {collapsed: boolean, lang: string} => {
  const  {collapsed} = siderState;
  const  {lang} = headerState;
  return {collapsed, lang};
}

export default withRouter(connect(mapStateToProps, {onLayoutBreakpoint})(AspianSider));
