import { IPost } from '../../../../models/aspian-core/post';
import {
  PostAction,
  PostActionTypesEnum,
} from '../../../actions/aspian-core/post/types';

export interface IPostState {
    posts: IPost[],
    loadingInitial: boolean
}

const initialState: IPostState = {
    posts: [],
    loadingInitial: true
}

export const postReducer = (state: IPostState = initialState, action: PostAction) => {
  const { type, payload } = action;
  switch (type) {
    case PostActionTypesEnum.getPosts:
      return {posts: payload, loadingInitial: false};
    default:
      return state;
  }
};
