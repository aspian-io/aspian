import React, { useContext } from 'react';
import { Layout } from 'antd';
import AspianMenu from './menu/Menu';
import { observer } from 'mobx-react-lite';
import { LanguageActionTypeEnum } from '../../../../app/stores/aspian-core/locale/types';
import { CoreRootStoreContext } from '../../../../app/stores/aspian-core/CoreRootStore';

const { Sider } = Layout;

const AspianSider = () => {
  // Stores
  const coreRootStore = useContext(CoreRootStoreContext);
  const {siderStore, localeStore} = coreRootStore;

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
