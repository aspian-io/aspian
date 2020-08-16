import { combineReducers } from 'redux';
import {postReducer} from './aspian-core/post/posts';
import {siderReducer, ISiderState} from './aspian-core/layout/sider';
import {IHeaderState, headerReducer} from './aspian-core/layout/header';
import { IPostState } from './aspian-core/post/posts';

export interface IStoreState {
    postsState: IPostState;
    siderState: ISiderState;
    headerState: IHeaderState;
}

export default combineReducers<IStoreState>({
    postsState: postReducer,
    siderState: siderReducer,
    headerState: headerReducer
});
