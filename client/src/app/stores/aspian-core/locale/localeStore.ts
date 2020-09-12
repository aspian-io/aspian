import { observable, action, configure, runInAction } from 'mobx';
import { CoreRootStore } from '../CoreRootStore';
import {
  LocaleVariableEnum,
  LanguageActionTypeEnum,
  DirectionActionTypeEnum,
} from './types';

configure({ enforceActions: 'observed' });

export default class LocaleStore {
  coreRootStore: CoreRootStore;
  constructor(coreRootStore: CoreRootStore) {
    this.coreRootStore = coreRootStore;
  }

  // Convert local storage string value to an anum key for language
  localStorageInitialLang =
    localStorage.getItem(LocaleVariableEnum.ASPIAN_CMS_LANG) !== null
      ? (localStorage.getItem(
          LocaleVariableEnum.ASPIAN_CMS_LANG
        ) as keyof typeof LanguageActionTypeEnum)
      : LanguageActionTypeEnum.en;
  // Convert local storage string value to an anum key for direction
  localStorageInitialDir =
    localStorage.getItem(LocaleVariableEnum.ASPIAN_CMS_DIR) !== null
      ? (localStorage.getItem(
          LocaleVariableEnum.ASPIAN_CMS_DIR
        ) as keyof typeof DirectionActionTypeEnum)
      : DirectionActionTypeEnum.LTR;

  // Convert local storage string value to enum for language and saving it in initialLang const for further usage
  initialLang = LanguageActionTypeEnum[this.localStorageInitialLang];
  // Convert local storage string value to enum for direction and saving it in initialDir const for further usage
  initialDir = DirectionActionTypeEnum[this.localStorageInitialDir];

  @observable lang: LanguageActionTypeEnum = this.initialLang;
  @observable dir: DirectionActionTypeEnum = this.initialDir;
  

  @action handleChangeLanguage = (
    lang: LanguageActionTypeEnum = LanguageActionTypeEnum.en
  ) => {
    localStorage.setItem(LocaleVariableEnum.ASPIAN_CMS_LANG, lang);

    // For English language
    if (lang === LanguageActionTypeEnum.en) {
      localStorage.setItem(
        LocaleVariableEnum.ASPIAN_CMS_DIR,
        DirectionActionTypeEnum.LTR
      );

      runInAction('change language and dir to LTR', () => {
        this.lang = lang;
        this.dir = DirectionActionTypeEnum.LTR;
      });
    }

    // For Persian (Farsi) language
    if (lang === LanguageActionTypeEnum.fa) {
      localStorage.setItem(
        LocaleVariableEnum.ASPIAN_CMS_DIR,
        DirectionActionTypeEnum.RTL
      );

      runInAction('change language and dir to RTL', () => {
        this.lang = lang;
        this.dir = DirectionActionTypeEnum.RTL;
      });
    }
  };
}
