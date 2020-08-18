import { Dispatch } from 'redux';
import agent from '../../../../api/aspian-core/agent';
import { PostActionTypesEnum, IGetPostsAction, ILoadingPostsAction } from './types';


export const setLoading = (isLoading: boolean = true) => async (dispatch: Dispatch) => {
  dispatch<ILoadingPostsAction>({
    type: PostActionTypesEnum.LOADING_POST,
    loadingInitial: isLoading
  });
}

export const getPostsEnvelope = (limit: number = 2, page: number) => async (dispatch: Dispatch) => {
  const postsEnvelope = await agent.Posts.list(limit, page);
  
  dispatch<IGetPostsAction>({
    
    type: PostActionTypesEnum.GET_POSTS,
    postsEnvelope: postsEnvelope,
  });
};


