import { combineReducers } from 'redux';
import { postReducer } from './aspian-core/post/posts';
import { siderReducer, ISiderState } from './aspian-core/layout/sider';
import { ILocaleState, localeReducer } from './aspian-core/locale/locale';
import { IPostState } from './aspian-core/post/posts';

export interface IStoreState {
  postsState: IPostState;
  siderState: ISiderState;
  localeState: ILocaleState;
}

export default combineReducers<IStoreState>({
  postsState: postReducer,
  siderState: siderReducer,
  localeState: localeReducer,
});
