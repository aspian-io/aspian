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
import { withTranslation, WithTranslation } from 'react-i18next';
import { e2p } from '../../../../utils/aspian-core/base/numberConverter';
import { GetRoundedFileSize } from '../../../../utils/aspian-core/base/fileSize';
import { v4 as uuidv4 } from 'uuid';
import { UAParser } from 'ua-parser-js';
import { history } from '../../../..';
import '../../../../scss/aspian-core/pages/posts/post-details/_post-details.scss';
import { TaxonomyTypeEnum } from '../../../../app/models/aspian-core/post';
import agent from '../../../../app/api/aspian-core/agent';
import { CoreRootStoreContext } from '../../../../app/stores/aspian-core/CoreRootStore';

const { TabPane } = Tabs;

type Props = WithTranslation & RouteComponentProps<DetailParams>;

interface DetailParams {
  id: string;
}

const PostDetails: FC<Props> = ({ match, t }) => {
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

  // Gets random avatar colors from colors in the colors array
  const getAvatarBgColor = () => {
    const colors = ['#f56a00', '#7265e6', '#ffbf00', '#00a2ae'];
    const randomColorIndex = Math.floor(Math.random() * 3.9);
    return colors[randomColorIndex];
  };

  // To delete a post
  const ondDeleteBtnClick = async (id: string) => {
    await deletePost(id);
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
          Edit
        </Button>,
        <Popconfirm
          key={uuidv4()}
          title={t('post-list.button.delete.popConfirm.single-item-title')}
          onConfirm={() => ondDeleteBtnClick(post!.id)}
          okText={t('post-list.button.delete.popConfirm.okText')}
          cancelText={t('post-list.button.delete.popConfirm.cancelText')}
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
            Delete
          </Button>
        </Popconfirm>,
      ]}
      footer={
        <Tabs defaultActiveKey="1">
          <TabPane tab="Attachments" key="1">
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
                        <Descriptions.Item label="File Name">
                          <a
                            href="http://"
                            target="_blank"
                            rel="noopener noreferrer"
                          >
                            {value.attachment.fileName}
                          </a>
                        </Descriptions.Item>
                        <Descriptions.Item label="File Type">
                          <Tag color="error">{value.attachment.type}</Tag>
                        </Descriptions.Item>
                        <Descriptions.Item label="Size">
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
          <TabPane tab="Statistics" key="2">
            <div style={{ margin: '1.5rem 0' }}>
              <Descriptions
                size="small"
                bordered
                column={{ md: 3, sm: 2, xs: 1 }}
              >
                <Descriptions.Item label="View Count">
                  {lang === LanguageActionTypeEnum.fa
                    ? e2p(post!.viewCount.toString())
                    : post!.viewCount}
                </Descriptions.Item>
                <Descriptions.Item label="Comments">
                  {lang === LanguageActionTypeEnum.fa
                    ? e2p(post!.comments.toString())
                    : post!.comments}
                </Descriptions.Item>
                <Descriptions.Item label="Child Posts">
                  {lang === LanguageActionTypeEnum.fa
                    ? e2p(post!.childPosts.toString())
                    : post!.childPosts}
                </Descriptions.Item>
              </Descriptions>
            </div>
          </TabPane>
          <TabPane tab="Author Info" key="3">
            <div style={{ margin: '1.5rem 0' }}>
              {post.createdBy ? (
                <Card className="user-info-card" hoverable>
                  <Card.Meta
                    avatar={
                      <Avatar
                        src={agent.Attachments.getImageUrl(
                          post.createdBy?.profilePhoto.fileName
                        )}
                        style={
                          post.createdBy?.profilePhoto
                            ? { backgroundColor: 'initial' }
                            : { backgroundColor: getAvatarBgColor() }
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
                    <Descriptions.Item label="Username">
                      {post.createdBy.userName}
                    </Descriptions.Item>
                    <Descriptions.Item label="Email Address">
                      {post!.createdBy?.email}
                    </Descriptions.Item>
                    <Descriptions.Item label="Bio">
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
          <TabPane tab="Editor Info" key="4">
            <div style={{ margin: '1.5rem 0' }}>
              {post.modifiedBy ? (
                <Card className="user-info-card" hoverable>
                  <Card.Meta
                    avatar={
                      <Avatar
                        src={agent.Attachments.getImageUrl(
                          post.modifiedBy?.profilePhoto.fileName
                        )}
                        style={
                          post.modifiedBy?.profilePhoto
                            ? { backgroundColor: 'initial' }
                            : { backgroundColor: getAvatarBgColor() }
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
                    <Descriptions.Item label="Username">
                      {post!.modifiedBy?.userName}
                    </Descriptions.Item>
                    <Descriptions.Item label="Email Address">
                      {post!.modifiedBy?.email}
                    </Descriptions.Item>
                    <Descriptions.Item label="Bio">
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
          <TabPane tab="User Agent" key="5">
            <div style={{ margin: '1.5rem 0' }}>
              <Descriptions
                size="small"
                bordered
                column={{ xxl: 4, xl: 3, lg: 3, md: 3, sm: 2, xs: 1 }}
              >
                <Descriptions.Item label="Device">
                  {ua.getDevice().type ?? 'Desktop'}
                </Descriptions.Item>
                <Descriptions.Item label="OS">
                  {ua.getOS().name} {ua.getOS().version}
                </Descriptions.Item>
                <Descriptions.Item label="Browser">
                  {ua.getBrowser().name} {ua.getBrowser().version}
                </Descriptions.Item>
                <Descriptions.Item label="IP Address">
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
            <Descriptions.Item label="Created By">
              {post.createdBy?.displayName}
            </Descriptions.Item>
            <Descriptions.Item label="Created At">
              {post.createdAt}
            </Descriptions.Item>
            <Descriptions.Item label="Modified By">
              {post.modifiedBy?.displayName}
            </Descriptions.Item>
            <Descriptions.Item label="Modified At">
              {post.modifiedAt}
            </Descriptions.Item>
            <Descriptions.Item label="Status">
              {post.postStatus}
            </Descriptions.Item>
            <Descriptions.Item label="Slug">{post.slug}</Descriptions.Item>
            <Descriptions.Item label="Is Pinned">
              {post.isPinned ? (
                <CheckOutlined style={{ color: '#52c41a' }} />
              ) : (
                <CloseOutlined style={{ color: '#f5222d' }} />
              )}
            </Descriptions.Item>
            <Descriptions.Item label="Pin Order">
              {post.pinOrder}
            </Descriptions.Item>
            <Descriptions.Item label="Order">1</Descriptions.Item>
            <Descriptions.Item label="Comment Allowed">
              {post.commentAllowed ? (
                <CheckOutlined style={{ color: '#52c41a' }} />
              ) : (
                <CloseOutlined style={{ color: '#f5222d' }} />
              )}
            </Descriptions.Item>
            <Descriptions.Item label="Categories">
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
            <Descriptions.Item label="Tags">
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
          <Divider>Post Content</Divider>
          <Paragraph ellipsis={{ rows: 4, expandable: true, symbol: 'more' }}>
            {post!.content}
          </Paragraph>
        </div>
      </div>
    </PageHeader>
  );
};

export default withTranslation()(observer(PostDetails));
