import { observable, action, configure, runInAction } from 'mobx';
import { createContext } from 'react';

configure({ enforceActions: 'observed' });

class SiderStore {
  @observable collapsed = false;

  @action toggle = (collapsed: boolean) => {
    runInAction('toggle action', () => {
      this.collapsed = !collapsed;
    });

    const contentLayoutDirectionIsRtl = document
      .getElementById('contentLayout')!
      .classList.contains('ant-layout-rtl');

    if (collapsed) {
      if (!contentLayoutDirectionIsRtl)
        document.getElementById('contentLayout')!.style.marginLeft = '200px';
      else
        document.getElementById('contentLayout')!.style.marginRight = '200px';
    } else {
      if (!contentLayoutDirectionIsRtl)
        document.getElementById('contentLayout')!.style.marginLeft = '0';
      else document.getElementById('contentLayout')!.style.marginRight = '0';
    }
  };

  @action onLayoutBreakpoint = (broken: boolean, isRtl: boolean) => {
    if (broken) {
      if (document.getElementById('contentLayout')) {
        document.getElementById('contentLayout')!.style.marginLeft = '0';
        document.getElementById('contentLayout')!.style.marginRight = '0';
        document.getElementById('appLayout')!.style.overflow = 'hidden';
        document.getElementById('contentLayout')!.style.minWidth = `100%`;
      }

      runInAction('onLayoutBroken action - broken', () => {
        this.collapsed = broken;
      });
    } else {
      if (document.getElementById('contentLayout')) {
        if (isRtl) {
          document.getElementById('contentLayout')!.style.marginRight = '200px';
          document.getElementById('contentLayout')!.style.marginLeft = '0';
        } else {
          document.getElementById('contentLayout')!.style.marginLeft = '200px';
          document.getElementById('contentLayout')!.style.marginRight = '0';
        }

        document.getElementById('appLayout')!.style.overflow = 'initial';
        document.getElementById('contentLayout')!.style.minWidth = `initial`;
      }

      runInAction('onLayoutBroken action - not broken', () => {
        this.collapsed = broken;
      });
    }
  };
}

export default createContext(new SiderStore());
