/*****************************
 ****** Action Type Enum *****
 *****************************/
export enum LayoutActionTypesEnum {
    TOGGLE_SIDER = "TOGGLE_SIDER",
    COLLAPSE_SIDER = "COLLAPSE_SIDER",
    CHANGE_LANG = "CHANGE_LANG"
}

/*****************************
 ***** Action Type Alias *****
 *****************************/
export type SiderAction = IToggleSiderAction | ISiderOnBreakpointsCollapse;
export type HeaderAction = ISetLanguage;

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

// Change Languge of the website
export interface ISetLanguage {
    type: LayoutActionTypesEnum.CHANGE_LANG,
    payload: string
}