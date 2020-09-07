import React, { useEffect, FC, useContext, Fragment } from 'react';
import {
  PageHeader,
  Tabs,
  Button,
  Descriptions,
  Tag,
  Divider,
  Card,
  Popconfirm,
  Spin,
  Avatar,
  Empty,
  message,
} from 'antd';
import {
  EditFilled,
  DeleteFilled,
  CheckOutlined,
  CloseOutlined,
} from '@ant-design/icons';
import Paragraph from 'antd/lib/typography/Paragraph';
import { RouteComponentProps } from 'react-router-dom';
import { LanguageActionTypeEnum } from '../../../../app/stores/aspian-core/locale/types';
import { observer } from 'mobx-react-lite';
import { useTranslation } from 'react-i18next';
import { e2p, ConvertDigitsToCurrentLanguage } from '../../../../utils/aspian-core/base/NumberConverter';
import { GetRoundedFileSize } from '../../../../utils/aspian-core/base/FileSize';
import { v4 as uuidv4 } from 'uuid';
import { UAParser } from 'ua-parser-js';
import { history } from '../../../..';
import '../../../../scss/aspian-core/pages/posts/post-details/_post-details.scss';
import { TaxonomyTypeEnum } from '../../../../app/models/aspian-core/post';
import agent from '../../../../app/api/aspian-core/agent';
import { CoreRootStoreContext } from '../../../../app/stores/aspian-core/CoreRootStore';
import GetRandomColor from '../../../../utils/aspian-core/base/GetRandomColor';
import moment from 'moment';
import jalaliMoment from 'jalali-moment';

const { TabPane } = Tabs;

interface DetailParams {
  id: string;
}

const PostDetails: FC<RouteComponentProps<DetailParams>> = ({ match }) => {
  const { t } = useTranslation('core_postDetails');
  /// Stores
  const coreRootStore = useContext(CoreRootStoreContext);
  const {
    getPost,
    deletePost,
    post,
    submitting,
    loadingInitial,
  } = coreRootStore.postStore;
  const { lang } = coreRootStore.localeStore;

  useEffect(() => {
    getPost(match.params.id);
  }, [getPost, match.params.id]);

  // Shows loading indicator if post hasn't been loaded completely yet
  if (loadingInitial || !post) {
    return (
      <div className="spinner-wrapper">
        <Spin wrapperClassName="spinner-wrapper" />
      </div>
    );
  }

  // To delete a post
  const ondDeleteBtnClick = async (id: string) => {
    try {
      await deletePost(id);
      message.success(t('messages.post-deleting-success'));
    } catch (error) {
      message.error(t('messages.post-deleting-error'));
    }
  };

  // Initializing UA Parser
  const ua = new UAParser();
  ua.setUA(post.userAgent);

  return (
    <PageHeader
      className="site-page-header-responsive"
      onBack={() => history.push('/admin/posts')}
      title={post.title}
      subTitle={post.subtitle}
      extra={[
        <Button
          key={uuidv4()}
          size="small"
          type="primary"
          icon={<EditFilled />}
        >
          {t('buttons.edit')}
        </Button>,
        <Popconfirm
          key={uuidv4()}
          title={t('popconfirm.title')}
          onConfirm={() => ondDeleteBtnClick(post!.id)}
          okText={t('popconfirm.ok-text')}
          cancelText={t('popconfirm.cancel-text')}
          placement={lang === LanguageActionTypeEnum.en ? 'left' : 'right'}
          okButtonProps={{
            danger: true,
          }}
        >
          <Button
            loading={submitting}
            size="small"
            type="primary"
            icon={<DeleteFilled />}
            danger
          >
            {t('buttons.delete')}
          </Button>
        </Popconfirm>,
      ]}
      footer={
        <Tabs defaultActiveKey="1">
          <TabPane tab={t('tabs.attachments.name')} key="1">
            <div style={{ margin: '1.5rem 0' }}>
              {post.postAttachments.length > 0 ? (
                post!.postAttachments.map((value, i) => {
                  return (
                    <Fragment key={uuidv4()}>
                      <Descriptions
                        size="small"
                        bordered
                        column={{ sm: 1, xs: 1 }}
                      >
                        <Descriptions.Item
                          label={t('tabs.attachments.content.file-name')}
                        >
                          <a
                            href={agent.Attachments.getFileUrl(
                              value.attachment.fileName
                            )}
                            target="_self"
                            rel="noopener noreferrer"
                          >
                            {value.attachment.fileName}
                          </a>
                        </Descriptions.Item>
                        <Descriptions.Item
                          label={t('tabs.attachments.content.file-type')}
                        >
                          <Tag color="error">{value.attachment.type}</Tag>
                        </Descriptions.Item>
                        <Descriptions.Item
                          label={t('tabs.attachments.content.file-size')}
                        >
                          {GetRoundedFileSize(value.attachment.fileSize, lang)}
                        </Descriptions.Item>
                      </Descriptions>
                      <br />
                    </Fragment>
                  );
                })
              ) : (
                <Empty />
              )}
            </div>
          </TabPane>
          <TabPane tab={t('tabs.statistics.name')} key="2">
            <div style={{ margin: '1.5rem 0' }}>
              <Descriptions
                size="small"
                bordered
                column={{ md: 3, sm: 2, xs: 1 }}
              >
                <Descriptions.Item
                  label={t('tabs.statistics.content.view-count')}
                >
                  {lang === LanguageActionTypeEnum.fa
                    ? e2p(post!.viewCount.toString())
                    : post!.viewCount}
                </Descriptions.Item>
                <Descriptions.Item
                  label={t('tabs.statistics.content.comments')}
                >
                  {lang === LanguageActionTypeEnum.fa
                    ? e2p(post!.comments.toString())
                    : post!.comments}
                </Descriptions.Item>
                <Descriptions.Item
                  label={t('tabs.statistics.content.child-posts')}
                >
                  {lang === LanguageActionTypeEnum.fa
                    ? e2p(post!.childPosts.toString())
                    : post!.childPosts}
                </Descriptions.Item>
              </Descriptions>
            </div>
          </TabPane>
          <TabPane tab={t('tabs.author-info.name')} key="3">
            <div style={{ margin: '1.5rem 0' }}>
              {post.createdBy ? (
                <Card className="user-info-card" hoverable>
                  <Card.Meta
                    avatar={
                      <Avatar
                        src={agent.Attachments.getFileUrl(
                          post.createdBy?.profilePhoto?.fileName
                        )}
                        style={
                          post.createdBy?.profilePhoto
                            ? { backgroundColor: 'initial' }
                            : { backgroundColor: GetRandomColor() }
                        }
                        size="large"
                      >
                        {post.createdBy?.profilePhoto ??
                        post.createdBy?.displayName
                          ? post.createdBy?.displayName
                          : ''}
                      </Avatar>
                    }
                    title={post!.createdBy?.displayName}
                  />
                  <br />
                  <Descriptions size="small" column={{ sm: 1, xs: 1 }}>
                    <Descriptions.Item
                      label={t('tabs.author-info.content.username')}
                    >
                      {post.createdBy.userName}
                    </Descriptions.Item>
                    <Descriptions.Item
                      label={t('tabs.author-info.content.email-address')}
                    >
                      {post!.createdBy?.email}
                    </Descriptions.Item>
                    <Descriptions.Item
                      label={t('tabs.author-info.content.bio')}
                    >
                      <Paragraph
                        ellipsis={{ rows: 4, expandable: true, symbol: 'more' }}
                      >
                        {post!.createdBy?.bio}
                      </Paragraph>
                    </Descriptions.Item>
                  </Descriptions>
                </Card>
              ) : (
                <Empty />
              )}
            </div>
          </TabPane>
          <TabPane tab={t('tabs.editor-info.name')} key="4">
            <div style={{ margin: '1.5rem 0' }}>
              {post.modifiedBy ? (
                <Card className="user-info-card" hoverable>
                  <Card.Meta
                    avatar={
                      <Avatar
                        src={agent.Attachments.getFileUrl(
                          post.modifiedBy?.profilePhoto?.fileName
                        )}
                        style={
                          post.modifiedBy?.profilePhoto
                            ? { backgroundColor: 'initial' }
                            : { backgroundColor: GetRandomColor() }
                        }
                        size="large"
                      >
                        {post.modifiedBy?.profilePhoto ??
                        post.modifiedBy?.displayName
                          ? post.modifiedBy?.displayName
                          : ''}
                      </Avatar>
                    }
                    title={post!.modifiedBy?.displayName}
                  />
                  <br />
                  <Descriptions size="small" column={{ sm: 1, xs: 1 }}>
                    <Descriptions.Item
                      label={t('tabs.editor-info.content.username')}
                    >
                      {post!.modifiedBy?.userName}
                    </Descriptions.Item>
                    <Descriptions.Item
                      label={t('tabs.editor-info.content.email-address')}
                    >
                      {post!.modifiedBy?.email}
                    </Descriptions.Item>
                    <Descriptions.Item
                      label={t('tabs.editor-info.content.bio')}
                    >
                      <Paragraph
                        ellipsis={{ rows: 4, expandable: true, symbol: 'more' }}
                      >
                        {post!.modifiedBy?.bio}
                      </Paragraph>
                    </Descriptions.Item>
                  </Descriptions>
                </Card>
              ) : (
                <Empty />
              )}
            </div>
          </TabPane>
          <TabPane tab={t('tabs.user-agent.name')} key="5">
            <div style={{ margin: '1.5rem 0' }}>
              <Descriptions
                size="small"
                bordered
                column={{ xxl: 4, xl: 3, lg: 3, md: 3, sm: 2, xs: 1 }}
              >
                <Descriptions.Item label={t('tabs.user-agent.content.device')}>
                  {ua.getDevice().type ?? 'Desktop'}
                </Descriptions.Item>
                <Descriptions.Item label={t('tabs.user-agent.content.os')}>
                  {ua.getOS().name} {ua.getOS().version}
                </Descriptions.Item>
                <Descriptions.Item label={t('tabs.user-agent.content.browser')}>
                  {ua.getBrowser().name} {ua.getBrowser().version}
                </Descriptions.Item>
                <Descriptions.Item
                  label={t('tabs.user-agent.content.ip-address')}
                >
                  {lang === LanguageActionTypeEnum.fa
                    ? e2p(post.userIPAddress)
                    : post.userIPAddress}
                </Descriptions.Item>
              </Descriptions>
            </div>
          </TabPane>
        </Tabs>
      }
    >
      <div className="content">
        <div className="main">
          <Descriptions size="small" column={{ sm: 2, xs: 1 }}>
            <Descriptions.Item label={t('header.created-by')}>
              {post.createdBy?.displayName}
            </Descriptions.Item>
            <Descriptions.Item label={t('header.created-at')}>
              {lang === LanguageActionTypeEnum.fa
                ? e2p(
                    jalaliMoment(post.createdAt)
                      .locale('fa')
                      .format('YYYY-MM-DD HH:mm:ss')
                  )
                : moment(post.createdAt).format('YYYY-MM-DD HH:mm:ss')}
            </Descriptions.Item>
            <Descriptions.Item label={t('header.modified-by')}>
              {post.modifiedBy?.displayName}
            </Descriptions.Item>
            <Descriptions.Item label={t('header.modified-at')}>
              {lang === LanguageActionTypeEnum.fa && post.modifiedAt
                ? e2p(
                    jalaliMoment(post.modifiedAt)
                      .locale('fa')
                      .format('YYYY-MM-DD HH:mm:ss')
                  )
                : post.modifiedAt && moment(post.modifiedAt).format('YYYY-MM-DD HH:mm:ss')}
            </Descriptions.Item>
            <Descriptions.Item label={t('header.status')}>
              {post.postStatus}
            </Descriptions.Item>
            <Descriptions.Item label={t('header.slug')}>
              {post.slug}
            </Descriptions.Item>
            <Descriptions.Item label={t('header.is-pinned')}>
              {post.isPinned ? (
                <CheckOutlined style={{ color: '#52c41a' }} />
              ) : (
                <CloseOutlined style={{ color: '#f5222d' }} />
              )}
            </Descriptions.Item>
            <Descriptions.Item label={t('header.pin-order')}>
              {ConvertDigitsToCurrentLanguage(post.pinOrder, LanguageActionTypeEnum.en, lang)}
            </Descriptions.Item>
            <Descriptions.Item label={t('header.order')}>
              {ConvertDigitsToCurrentLanguage(post.order, LanguageActionTypeEnum.en, lang)}
            </Descriptions.Item>
            <Descriptions.Item label={t('header.comment-allowed')}>
              {post.commentAllowed ? (
                <CheckOutlined style={{ color: '#52c41a' }} />
              ) : (
                <CloseOutlined style={{ color: '#f5222d' }} />
              )}
            </Descriptions.Item>
            <Descriptions.Item label={t('header.categories')}>
              <div>
                {post!.taxonomyPosts.map((tp, i) => {
                  if (tp.taxonomy.type === TaxonomyTypeEnum.category) {
                    return (
                      <Tag key={uuidv4()} color="magenta">
                        {tp.taxonomy.term.name}
                      </Tag>
                    );
                  } else {
                    return '';
                  }
                })}
              </div>
            </Descriptions.Item>
            <Descriptions.Item label={t('header.tags')}>
              <div>
                {post!.taxonomyPosts.map((tp, i) => {
                  if (tp.taxonomy.type === TaxonomyTypeEnum.tag) {
                    return (
                      <Tag key={uuidv4()} color="cyan">
                        {tp.taxonomy.term.name}
                      </Tag>
                    );
                  } else {
                    return '';
                  }
                })}
              </div>
            </Descriptions.Item>
          </Descriptions>
        </div>
        <div style={{ margin: '2rem 0' }}>
          <Divider>{t('post-content')}</Divider>
          <Paragraph ellipsis={{ rows: 4, expandable: true, symbol: 'more' }}>
            {post!.content}
          </Paragraph>
        </div>
      </div>
    </PageHeader>
  );
};

export default observer(PostDetails);
