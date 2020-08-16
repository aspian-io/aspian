import {
  SiderAction,
  LayoutActionTypesEnum,
} from '../../../actions/aspian-core/layout/types';

export interface ISiderState {
    readonly collapsed : boolean;
};

const initialState: ISiderState = {
    collapsed: false
};

export const siderReducer = (
  state: ISiderState = initialState,
  action: SiderAction
) => {
  const { type, payload } = action;

  switch (type) {
    case LayoutActionTypesEnum.TOGGLE_SIDER:
    case LayoutActionTypesEnum.COLLAPSE_SIDER:
      return {collapsed: payload };
    
    default:
      return state;
  }
};
