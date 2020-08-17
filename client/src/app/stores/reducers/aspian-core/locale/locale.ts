import {
  LocaleAction,
  LanguageActionTypeEnum,
  DirectionActionTypeEnum,
  LocaleVariableEnum,
} from '../../../actions/aspian-core/locale/types';

export interface ILocaleState {
  readonly lang: LanguageActionTypeEnum;
  readonly dir: DirectionActionTypeEnum;
}

// Convert local storage string value to an anum key for language
const localStorageInitialLang =
  localStorage.getItem(LocaleVariableEnum.ASPIAN_CMS_LANG) !== null
    ? (localStorage.getItem(
        LocaleVariableEnum.ASPIAN_CMS_LANG
      ) as keyof typeof LanguageActionTypeEnum)
    : LanguageActionTypeEnum.en;
// Convert local storage string value to an anum key for direction
const localStorageInitialDir =
  localStorage.getItem(LocaleVariableEnum.ASPIAN_CMS_DIR) !== null
    ? (localStorage.getItem(
        LocaleVariableEnum.ASPIAN_CMS_DIR
      ) as keyof typeof DirectionActionTypeEnum)
    : DirectionActionTypeEnum.LTR;

// Convert local storage string value to enum for language and saving it in initialLang const for further usage
const initialLang = LanguageActionTypeEnum[localStorageInitialLang];
// Convert local storage string value to enum for direction and saving it in initialDir const for further usage
const initialDir = DirectionActionTypeEnum[localStorageInitialDir];

// Initialize initialState with values we got from the local storage and just converted them to their enum equivalents
const initialState: ILocaleState = {
  lang: initialLang,
  dir: initialDir,
};

export const localeReducer = (
  state: ILocaleState = initialState,
  action: LocaleAction
) => {
  const { type, payload } = action;

  switch (type) {
    case LanguageActionTypeEnum.en:
      return payload;

    case LanguageActionTypeEnum.fa:
      return payload;

    default:
      return state;
  }
};
