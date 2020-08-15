import { Dispatch } from 'redux';
import agent from '../../../../api/aspian-core/agent';
import { PostActionTypesEnum, IGetPostsAction } from './types';

export const getPosts = () => async (dispatch: Dispatch) => {
  const posts = await agent.Posts.list();

  dispatch<IGetPostsAction>({
    type: PostActionTypesEnum.getPosts,
    payload: posts,
  });
};
