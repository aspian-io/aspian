/*****************************
 ****** Action Type Enum *****
 *****************************/
export enum LocaleVariableEnum {
    ASPIAN_CMS_LANG = "aspianCmsLang",
    ASPIAN_CMS_DIR = "aspianCmsDir",
}

export enum LanguageActionTypeEnum {
    en = "en",
    fa = "fa",
}

export enum DirectionActionTypeEnum {
    LTR = "LTR",
    RTL = "RTL",
}

/*****************************
 ***** Action Type Alias *****
 *****************************/
export type LocaleAction = ISetLanguage;

/*****************************
 ***** Action Interfaces *****
 *****************************/

// Change Languge of the website
export interface ISetLanguage {
    type: LanguageActionTypeEnum,
    payload: {lang: LanguageActionTypeEnum, dir: DirectionActionTypeEnum}
}