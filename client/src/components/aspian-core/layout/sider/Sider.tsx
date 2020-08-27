import React, { useContext } from 'react';
import { Layout } from 'antd';
import AspianMenu from './menu/Menu';
import { observer } from 'mobx-react-lite';
import SiderStore from '../../../../app/stores/aspian-core/layout/siderStore';
import LocaleStore from '../../../../app/stores/aspian-core/locale/localeStore';
import { LanguageActionTypeEnum } from '../../../../app/stores/aspian-core/locale/types';

const { Sider } = Layout;

const AspianSider = () => {
  // Stores
  const siderStore = useContext(SiderStore);
  const localeStore = useContext(LocaleStore);

  return (
    <Sider
      className='sider'
      breakpoint="lg"
      collapsedWidth= "0"
      trigger={null}
      collapsible
      collapsed={siderStore.collapsed}
      onBreakpoint= {(broken) => siderStore.onLayoutBreakpoint(broken, localeStore.lang === LanguageActionTypeEnum.en ? false : true)}
    >
      <AspianMenu />
    </Sider>
  );
};

export default observer(AspianSider);
