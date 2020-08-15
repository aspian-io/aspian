import { IPost } from "../../../../models/aspian-core/post";

/*****************************
 ****** Action Type Enum *****
 *****************************/
export enum PostActionTypesEnum {
    getPosts
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
    type: PostActionTypesEnum.getPosts;
    payload: IPost[]
}