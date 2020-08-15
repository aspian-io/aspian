import { combineReducers } from 'redux';
import {postReducer} from './aspian-core/post/posts';
import { IPostState } from './aspian-core/post/posts';

export interface IStoreState {
    postsState: IPostState
}

export default combineReducers<IStoreState>({
    postsState: postReducer
});
