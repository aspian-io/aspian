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

export type PostAction = ILoadingPostsAction | IGetPostsAction;

/*****************************
 ***** Action Interfaces *****
 *****************************/

// Loading Posts State
export interface ILoadingPostsAction {
  type: PostActionTypesEnum.LOADING_POST;
  payload: boolean;
}

// Get All Posts
export interface IGetPostsAction {
  type: PostActionTypesEnum.GET_POSTS;
  payload: IPostsEnvelope;
}
