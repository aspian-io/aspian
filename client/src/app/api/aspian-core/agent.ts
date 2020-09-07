import axios from 'axios';
import { IPost, IPostsEnvelope } from '../../models/aspian-core/post';
import { IUser, IUserFormValues } from '../../models/aspian-core/user';
import common from '../common';

const {
  axiosRequestInterceptorHandleSuccess,
  axiosRequestInterceptorHandleError,
  axiosResponseInterceptorHandleSuccess,
  axiosResponseInterceptorHandleError,
  baseURL,
  requests,
} = common;

axios.interceptors.request.use(
  axiosRequestInterceptorHandleSuccess,
  axiosRequestInterceptorHandleError
);

axios.interceptors.response.use(
  axiosResponseInterceptorHandleSuccess,
  axiosResponseInterceptorHandleError
);

const Attachments = {
  getFileUrl: (fileName: string): string =>
    axios.getUri({ url: `${baseURL}/v1/attachments/download/${fileName}` }),
};

const Posts = {
  list: (
    limit?: number,
    page?: number,
    filterKey: string = '',
    filterValue: string = '',
    field: string = '',
    order: string = '',
    startDate: string = '',
    endDate: string = '',
    startNumber: number | '' = '',
    endNumber: number | '' = ''
  ): Promise<IPostsEnvelope> =>
    requests.get(
      `/v1/posts?limit=${limit}&offset=${
        page ? page * limit! : 0
      }&field=${field}&order=${order}&filterKey=${filterKey}&filterValue=${filterValue}&startDate=${startDate}&endDate=${endDate}&startNumber=${startNumber}&endNumber=${endNumber}`
    ),
  details: (id: string): Promise<IPost> =>
    requests.get(`/v1/posts/details/${id}`),
  create: (post: IPost) => requests.post('/v1/posts/create', post),
  update: (post: IPost) => requests.put(`/v1/posts/edit/${post.id}`, post),
  delete: (ids: string[]) => requests.del(`/v1/posts/delete`, ids),
};

const User = {
  current: (): Promise<IUser> => requests.get('/v1/user'),
  login: (user: IUserFormValues): Promise<IUser> =>
    requests.post('/v1/user/login', user),
  register: (user: IUserFormValues): Promise<IUser> =>
    requests.post('/v1/user/register', user),
  refresh: (): Promise<IUser> => requests.get('/v1/user/refresh-token'),
  logout: (): Promise<void> => requests.post('/v1/user/logout', {}),
};

export default {
  Attachments,
  Posts,
  User,
};
