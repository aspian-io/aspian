/*****************************
 ****** Action Type Enum *****
 *****************************/
export enum LayoutActionTypesEnum {
    TOGGLE_SIDER = "TOGGLE_SIDER",
    COLLAPSE_SIDER = "COLLAPSE_SIDER",
}

/*****************************
 ***** Action Type Alias *****
 *****************************/
export type SiderAction = IToggleSiderAction | ISiderOnBreakpointsCollapse;

/*****************************
 ***** Action Interfaces *****
 *****************************/

// Toggle Sider
export interface IToggleSiderAction {
    type: LayoutActionTypesEnum.TOGGLE_SIDER;
    payload: boolean
}

// Sider on breakpoints collapsing status
export interface ISiderOnBreakpointsCollapse {
    type: LayoutActionTypesEnum.COLLAPSE_SIDER,
    payload: boolean
}