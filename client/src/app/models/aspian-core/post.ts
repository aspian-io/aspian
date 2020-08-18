export interface IPostsEnvelope {
  posts: IPost[];
  postCount: number;
}

export interface IPost {
  id: string;
  title: string;
  subtitle: string;
  excerpt: string;
  slug: string;
  postStatus: PostStatusEnum;
  commentAllowed: boolean;
  order: number;
  viewCount: number;
  type: PostTypeEnum;
  isPinned: boolean;
  pinOrder: number;
  postHistories: number;
  comments: number;
  childPosts: number;
  createdAt: Date;
  createdBy: User;
  modifiedAt: string;
  modifiedBy: User;
  userAgent: string;
  userIPAddress: string;
  postAttachments: number;
  taxonomyPosts: ITaxonomyPost[];
}

export enum PostStatusEnum {
  Publish,
  Future,
  Draft,
  Pending,
  Private,
  Trash,
  AutoDraft,
  Inherit,
}

export enum PostTypeEnum {
  Posts,
  Products,
  Pages,
}

interface User {
  id: string;
  displayName: string;
  userName: string;
  email: string;
  bio: string;
  role: string;
}

interface PostAttachment {
  isMain: boolean;
  attachment: IAttachment;
}

interface IAttachment {
  fileName: string;
  fileSize: string;
  mimeType: string;
}

export interface ITaxonomyPost {
    taxonomy: ITaxonomy;
}

interface ITaxonomy {
    id: string;
    type: TaxonomyTypeEnum;
    term: ITerm;
}

export enum TaxonomyTypeEnum {
  nav_menu,
  category,
  tag,
}

interface ITerm {
    id: string;
    name: string;
}
