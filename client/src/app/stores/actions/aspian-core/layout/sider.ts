import { Dispatch } from 'redux';
import {
  IToggleSiderAction,
  LayoutActionTypesEnum,
  ISiderOnBreakpointsCollapse,
} from './types';

export const toggle = (collapsed: boolean) => (dispatch: Dispatch) => {
  const contentLayoutDirectionIsRtl = document
    .getElementById('contentLayout')!
    .classList.contains('ant-layout-rtl');

  dispatch<IToggleSiderAction>({
    type: LayoutActionTypesEnum.TOGGLE_SIDER,
    payload: !collapsed,
  });

  if (collapsed) {
    if (!contentLayoutDirectionIsRtl)
      document.getElementById('contentLayout')!.style.marginLeft = '200px';
    else document.getElementById('contentLayout')!.style.marginRight = '200px';
  } else {
    if (!contentLayoutDirectionIsRtl)
      document.getElementById('contentLayout')!.style.marginLeft = '0';
    else document.getElementById('contentLayout')!.style.marginRight = '0';
  }
};

export const onLayoutBreakpoint = (broken: boolean, isRtl: boolean) => (
  dispatch: Dispatch
) => {
  if (broken) {
    document.getElementById('contentLayout')!.style.marginLeft = '0';
    document.getElementById('contentLayout')!.style.marginRight = '0';
    document.getElementById('appLayout')!.style.overflow = 'hidden';
    document.getElementById('contentLayout')!.style.minWidth = `100%`;

    dispatch<ISiderOnBreakpointsCollapse>({
      type: LayoutActionTypesEnum.COLLAPSE_SIDER,
      payload: broken,
    });
  } else {
    if (isRtl) {
      document.getElementById('contentLayout')!.style.marginRight = '200px';
      document.getElementById('contentLayout')!.style.marginLeft = '0';
    } else {
      document.getElementById('contentLayout')!.style.marginLeft = '200px';
      document.getElementById('contentLayout')!.style.marginRight = '0';
    }

    document.getElementById('appLayout')!.style.overflow = 'initial';
    document.getElementById('contentLayout')!.style.minWidth = `initial`;

    dispatch<ISiderOnBreakpointsCollapse>({
      type: LayoutActionTypesEnum.COLLAPSE_SIDER,
      payload: broken,
    });
  }
};
