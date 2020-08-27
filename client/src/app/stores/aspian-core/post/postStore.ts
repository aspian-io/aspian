import { observable, action, configure, runInAction, computed } from 'mobx';
import { createContext } from 'react';
import { IPost } from '../../../models/aspian-core/post';
import agent from '../../../api/aspian-core/agent';

configure({ enforceActions: 'observed' });

class PostStore {
  @observable postRegistry = new Map<string, IPost>();
  @observable posts: IPost[] = [];
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
        this.posts = postsEnvelope.posts;
        this.postCount = postsEnvelope.postCount;
        this.maxAttachmentsNumber = postsEnvelope.maxAttachmentsNumber;
        this.maxChildPosts = postsEnvelope.maxChildPosts;
        this.maxComments = postsEnvelope.maxComments;
        this.maxPostHistories = postsEnvelope.maxPostHistories;
        this.maxViewCount = postsEnvelope.maxViewCount;
        this.loadingInitial = false;
      });
    } catch (error) {
      runInAction('load all posts error', () => {
        this.loadingInitial = false;
      });
      console.log(error);
    }
  };

  @action deletePosts = async (ids: string[]) => {
    try {
      this.submitting = true;
      await agent.Posts.delete(ids);
      runInAction('deletePosts action - remove loading', async () => {
        this.submitting = false;
      });
    } catch (error) {
      console.log(error);
      runInAction('deletePosts action - remove loading', () => {
        this.submitting = false;
      });
    }
  };
}

export default createContext(new PostStore());
