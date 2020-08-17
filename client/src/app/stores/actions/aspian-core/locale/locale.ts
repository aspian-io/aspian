import { Dispatch } from 'redux';
import {
  ISetLanguage,
  LanguageActionTypeEnum,
  LocaleVariableEnum,
  DirectionActionTypeEnum,
} from './types';

export const handleChangeLanguage = (
  lang: LanguageActionTypeEnum = LanguageActionTypeEnum.en
) => (dispatch: Dispatch) => {
  localStorage.setItem(LocaleVariableEnum.ASPIAN_CMS_LANG, lang);

  // For English language
  if (lang === LanguageActionTypeEnum.en) {
    localStorage.setItem(
      LocaleVariableEnum.ASPIAN_CMS_DIR,
      DirectionActionTypeEnum.LTR
    );

    dispatch<ISetLanguage>({
      type: lang,
      payload: { lang: lang, dir: DirectionActionTypeEnum.LTR },
    });
  }

  // For Persian (Farsi) language
  if (lang === LanguageActionTypeEnum.fa) {
    localStorage.setItem(
      LocaleVariableEnum.ASPIAN_CMS_DIR,
      DirectionActionTypeEnum.RTL
    );

    dispatch<ISetLanguage>({
      type: lang,
      payload: { lang: lang, dir: DirectionActionTypeEnum.RTL },
    });
  }
};
