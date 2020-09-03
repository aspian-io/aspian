import { observable, action, configure, runInAction, computed } from 'mobx';
import { CoreRootStore } from '../CoreRootStore';
import { IPost } from '../../../models/aspian-core/post';
import agent from '../../../api/aspian-core/agent';
import { ResultStatusEnum } from '../layout/resultStore/types';
import { history } from '../../../..';
import { AxiosError } from 'axios';

configure({ enforceActions: 'observed' });

export default class PostStore {
  coreRootStore: CoreRootStore;
  constructor(coreRootStore: CoreRootStore) {
    this.coreRootStore = coreRootStore;
  }

  @observable postRegistry = new Map<string, IPost>();
  @observable post: IPost | undefined = undefined;
  @observable loadingInitial = true;
  @observable submitting = false;
  @observable postCount: number = 0;
  @observable maxAttachmentsNumber: number = 0;
  @observable maxViewCount: number = 0;
  @observable maxPostHistories: number = 0;
  @observable maxComments: number = 0;
  @observable maxChildPosts: number = 0;

  @computed get postsByDate() {
    return Array.from(this.postRegistry.values()).sort(
      (a, b) =>
        Date.parse(a.createdAt.toString()) - Date.parse(b.createdAt.toString())
    );
  }

  @action loadPosts = async (
    limit: number = 3,
    offset = 0,
    filterKey = '',
    filterValue = '',
    field = '',
    order = '',
    startDate = '',
    endDate = '',
    startNumber: number | '' = '',
    endNumber: number | '' = ''
  ) => {
    this.loadingInitial = true;
    try {
      const postsEnvelope = await agent.Posts.list(
        limit,
        offset,
        filterKey,
        filterValue,
        field,
        order,
        startDate,
        endDate,
        startNumber,
        endNumber
      );
      runInAction('loading all posts', () => {
        postsEnvelope.posts.map((i) => this.postRegistry.set(i.id, i));
        this.postCount = postsEnvelope.postCount;
        this.maxAttachmentsNumber = postsEnvelope.maxAttachmentsNumber;
        this.maxChildPosts = postsEnvelope.maxChildPosts;
        this.maxComments = postsEnvelope.maxComments;
        this.maxViewCount = postsEnvelope.maxViewCount;
      });
    } catch (error) {
      console.log(error);
    } finally {
      runInAction('load all posts remove loading - finally', () => {
        this.loadingInitial = false;
      });
    }
  };

  @action deletePosts = async (ids: string[]) => {
    try {
      this.submitting = true;
      await agent.Posts.delete(ids);
      runInAction('deletePosts action - remove loading', () => {
        ids.forEach((i) => this.postRegistry.delete(i));
      });
    } catch (error) {
      const axiosError = error as AxiosError;
      console.log(axiosError);
      this.coreRootStore.resultStore.setResultPage(
        ResultStatusEnum.Error,
        'The operation faild!',
        'Please check the following error message before trying again.',
        'Back to Posts',
        '/admin/posts',
        'Back to Dashboard',
        '/admin',
        [axiosError.message]
      );
      history.push('/admin/post-deletion-result');
    } finally {
      runInAction('deletePosts action - remove loading - finally', () => {
        this.submitting = false;
      });
    }
  };

  @action deletePost = async (id: string) => {
    try {
      this.submitting = true;
      await agent.Posts.delete([id]);
      runInAction('deletePost action - remove loading', () => {
        this.postRegistry.delete(id);
      });
      this.coreRootStore.resultStore.setResultPage(
        ResultStatusEnum.Success,
        'The post deleted successfully!',
        '',
        'Back to Posts',
        '/admin/posts'
      );
      history.push('/admin/post-deletion-result');
    } catch (error) {
      const axiosError = error as AxiosError;
      console.log(axiosError);
      this.coreRootStore.resultStore.setResultPage(
        ResultStatusEnum.Error,
        'The operation faild!',
        'Please check the following error message before trying again.',
        'Back to the post',
        `/admin/posts/details/${id}`,
        'Back to Dashboard',
        '/admin',
        [axiosError.message]
      );
      history.push('/admin/post-deletion-result');
    } finally {
      runInAction('deletePost action - remove loading - finally', () => {
        this.submitting = false;
      });
    }
  };

  private loadPost = (id: string) => {
    return this.postRegistry.get(id);
  };

  @action getPost = async (id: string) => {
    this.loadingInitial = true;
    const postFromRegistry = this.loadPost(id);
    if (postFromRegistry) {
      this.post = postFromRegistry;
      this.loadingInitial = false;
    } else {
      try {
        const loadedPost = await agent.Posts.details(id);
        runInAction('getPost action - initilizing post', () => {
          this.post = loadedPost;
          this.loadingInitial = false;
        });
      } catch (error) {
        console.log(error);
        runInAction('getPost action - remove loading - error', () => {
          this.loadingInitial = false;
        });
      }
    }
  };
}
