import { Dispatch } from 'redux';
import {ISetLanguage, LayoutActionTypesEnum} from './types';

export const handleChangeLang = (value: string) => (dispatch: Dispatch) => {
  localStorage.setItem('aspianCmsLang', value);

  dispatch<ISetLanguage>({
    type: LayoutActionTypesEnum.CHANGE_LANG,
    payload: value
  })
};
