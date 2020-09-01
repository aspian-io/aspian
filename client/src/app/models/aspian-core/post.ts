export interface IPostsEnvelope {
  posts: IPost[];
  postCount: number;
  maxAttachmentsNumber: number;
  maxViewCount: number;
  maxComments: number;
  maxChildPosts: number;
}

export interface IPost {
  id: string;
  title: string;
  subtitle: string;
  excerpt: string;
  content: string;
  slug: string;
  postStatus: PostStatusEnum;
  commentAllowed: boolean;
  order: number;
  viewCount: number;
  type: PostTypeEnum;
  isPinned: boolean;
  pinOrder: number;
  comments: number;
  childPosts: number;
  createdAt: Date;
  createdBy: User;
  modifiedAt: string;
  modifiedBy: User;
  userAgent: string;
  userIPAddress: string;
  postAttachments: PostAttachment[];
  taxonomyPosts: ITaxonomyPost[];
}

export enum PostStatusEnum {
  Publish = 'Publish',
  Future = 'Future',
  Draft = 'Draft',
  Pending = 'Pending',
  Private = 'Private',
  Trash = 'Trash',
  AutoDraft = 'AutoDraft',
  Inherit = 'Inherit',
}

export enum PostTypeEnum {
  Posts,
  Products,
  Pages,
}

export interface User {
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

export enum AttachmentTypeEnum {
  Photo,
  Video,
  Audio,
  PDF,
  TextFile,
  Compressed,
  Other,
}
export enum UploadLocationEnum {
  LocalHost,
  FtpServer,
}

interface IAttachment {
  type: AttachmentTypeEnum;
  fileName: string;
  fileSize: string;
  mimeType: string;
  uploadLocation: UploadLocationEnum;
  relativePath: string;
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
