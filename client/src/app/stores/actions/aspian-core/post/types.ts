import { IPost } from "../../../../models/aspian-core/post";

/*****************************
 ****** Action Type Enum *****
 *****************************/
export enum PostActionTypesEnum {
    GET_POSTS = 'GET_POSTS'
}

/*****************************
 ***** Action Type Alias *****
 *****************************/
export type PostAction = IGetPostsAction;

/*****************************
 ***** Action Interfaces *****
 *****************************/

// Get All Posts
export interface IGetPostsAction {
    type: PostActionTypesEnum.GET_POSTS;
    payload: IPost[]
}