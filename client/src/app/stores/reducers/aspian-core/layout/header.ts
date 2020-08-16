import {
  HeaderAction,
  LayoutActionTypesEnum,
} from '../../../actions/aspian-core/layout/types';

export interface IHeaderState {
  readonly lang: string;
}

const initialState: IHeaderState = {
  lang: localStorage.getItem("aspianCmsLang") ?? "en",
};

export const headerReducer = (
  state: IHeaderState = initialState,
  action: HeaderAction
) => {
  const { type, payload } = action;

  switch (type) {
      case LayoutActionTypesEnum.CHANGE_LANG:
          return {lang: payload}
  
      default:
          return state;
  }
};
