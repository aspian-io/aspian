import axios, { AxiosResponse, AxiosRequestConfig } from 'axios';
import { IUser } from '../models/aspian-core/user';
import { history } from '../..';
import { message } from 'antd';

const baseURL = 'http://localhost:5001/api';
axios.defaults.baseURL = baseURL;
axios.defaults.withCredentials = true;

let token: string | null = null;
let loggingIn: boolean = false;

const SetLogin = () => {
  loggingIn = true;
};

const SetLogout = () => {
  loggingIn = false;
};

const SetAuthHeader = (newToken: string) => {
  token = newToken;
};
const axiosRequestInterceptorHandleSuccess = async (
  config: AxiosRequestConfig
) => {
  if (!loggingIn && token === null && window.location.pathname !== '/login') {
    try {
      const response = await axios
        .create()
        .get<IUser>(`${baseURL}/v1/user/refresh-token`);
      token = response.data.token;
      config.headers.Authorization = `Bearer ${response.data.token}`;
    } catch (error) {
      loggingIn = false;
      token = null;
      history.push('/login');
      window.location.reload();
    } finally {
      return config;
    }
  }
  config.headers.Authorization = `Bearer ${token}`;
  return config;
};
const axiosRequestInterceptorHandleError = (error: any) => {
  return Promise.reject(error);
};

const axiosResponseInterceptorHandleSuccess = (response: AxiosResponse) => {
  return response;
};

const axiosResponseInterceptorHandleError = (error: any) => {
  if (error.message === 'Network Error' && !error.response) {
    message.error('Network Error!');
  }
  const { status } = error.response;
  const originalRequest = error.config;
  let res = error.response;
  if (status === 404) {
    history.push('/notfound');
  }
  if (status === 400) {
    history.push('/badrequest');
  }
  if (
    status === 401 &&
    res.config &&
    !res.config.__isRetryRequest &&
    window.location.pathname !== '/login'
  ) {
    return new Promise((resolve, reject) => {
      axios
        .create()
        .get<IUser>(`${baseURL}/v1/user/refresh-token`)
        .then((response) => {
          token = response.data.token;
          originalRequest.__isRetryRequest = true;
          originalRequest.headers.Authorization = `Bearer ${response.data.token}`;
          resolve(axios(originalRequest));
        })
        .catch((error) => {
          loggingIn = false;
          token = null;
          history.push('/login');
          window.location.reload();
          reject();
        });
    });
  }
  if (status === 403) {
    history.push('/unathorized403');
  }
  if (status === 500) {
    history.push('/server-error');
  }
  throw error.response;
};

const responseBody = (response: AxiosResponse) => response.data;

// Just for development mode
const sleep = (ms: number) => (response: AxiosResponse) =>
  new Promise<AxiosResponse>((resolve) =>
    setTimeout(() => resolve(response), ms)
  );

const requests = {
  get: (url: string) => axios.get(url).then(sleep(1000)).then(responseBody),
  post: (url: string, body: {}) =>
    axios.post(url, body).then(sleep(1000)).then(responseBody),
  put: (url: string, body: {}) =>
    axios.put(url, body).then(sleep(1000)).then(responseBody),
  del: (url: string, ids: string[]) =>
    axios
      .delete(url, { data: { ids: ids } })
      .then(sleep(1000))
      .then(responseBody),
};

export default {
  baseURL,
  requests,
  axiosRequestInterceptorHandleSuccess,
  axiosRequestInterceptorHandleError,
  axiosResponseInterceptorHandleSuccess,
  axiosResponseInterceptorHandleError,
  SetAuthHeader,
  SetLogin,
  SetLogout,
};
