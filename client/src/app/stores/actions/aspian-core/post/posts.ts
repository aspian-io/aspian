import { Dispatch } from 'redux';
import agent from '../../../../api/aspian-core/agent';
import { PostActionTypesEnum, IGetPostsAction, ILoadingPostsAction } from './types';


export const setLoading = (isLoading: boolean = true) => async (dispatch: Dispatch) => {
  dispatch<ILoadingPostsAction>({
    type: PostActionTypesEnum.LOADING_POST,
    payload: isLoading
  });
}

export const getPostsEnvelope = (limit: number = 3, offset: number = 0, filterKey?: string, filterValue?: string, field?: string, order?: string) => async (dispatch: Dispatch) => {
  const postsEnvelope = await agent.Posts.list(limit, offset, filterKey, filterValue, field, order);
  
  dispatch<IGetPostsAction>({
    type: PostActionTypesEnum.GET_POSTS,
    payload: postsEnvelope,
  });
};


