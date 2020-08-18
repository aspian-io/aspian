import { IPostsEnvelope } from '../../../../models/aspian-core/post';

/*****************************
 ****** Action Type Enum *****
 *****************************/
export enum PostActionTypesEnum {
  LOADING_POST = 'LOADING_POST',
  GET_POSTS = 'GET_POSTS',
  CLEAR_POSTS = 'CLEAR_POSTS',
}

/*****************************
 ***** Action Type Alias *****
 *****************************/
export type PostAction =
  | {
      type: PostActionTypesEnum.GET_POSTS;
      postsEnvelope: IPostsEnvelope;
      loadingInitial: boolean;
    }
  | {
      type: PostActionTypesEnum.LOADING_POST;
      loadingInitial: boolean;
    };

/*****************************
 ***** Action Interfaces *****
 *****************************/

// Loading Posts State
export interface ILoadingPostsAction {
  type: PostActionTypesEnum.LOADING_POST;
  loadingInitial: boolean;
}

// Get All Posts
export interface IGetPostsAction {
  type: PostActionTypesEnum.GET_POSTS;
  postsEnvelope: IPostsEnvelope;
}
