import { Dispatch } from 'redux';
import agent from '../../../../api/aspian-core/agent';
import { PostActionTypesEnum, IGetPostsAction, ILoadingPostsAction } from './types';


export const setLoading = (isLoading: boolean = true) => async (dispatch: Dispatch) => {
  dispatch<ILoadingPostsAction>({
    type: PostActionTypesEnum.LOADING_POST,
    payload: isLoading
  });
}

export const getPostsEnvelope = (limit: number = 3, offset: number = 0, filterKey: string = '', filterValue: string = '', field: string = '', order: string = '', startDate: string = '', endDate: string = '', startNumber: number | '' = '', endNumber: number | '' = '') => async (dispatch: Dispatch) => {
  const postsEnvelope = await agent.Posts.list(limit, offset, filterKey, filterValue, field, order, startDate, endDate, startNumber, endNumber);
  console.log("FilterKey log from action: ", filterKey);
  console.log("FilterValue log from action: ", filterValue);
  console.log("Start date log from action: ", startDate);
  console.log("End date log from action: ", endDate);
  console.log("Start number log from action: ", startNumber);
  console.log("End number log from action: ", endNumber);
  console.log("Sort field log from action: ", field);
  console.log("Sort order log from action: ", order);
  
  dispatch<IGetPostsAction>({
    type: PostActionTypesEnum.GET_POSTS,
    payload: postsEnvelope,
  });
};


