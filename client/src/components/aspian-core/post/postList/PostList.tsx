import React, { useState, ReactText, FC, useEffect } from 'react';
import { Table, Button, Space, Tooltip } from 'antd';
import { TableRowSelection } from 'antd/lib/table/interface';
import { ColumnType } from 'antd/lib/table';
import { EditFilled, DeleteFilled, ClockCircleFilled } from '@ant-design/icons';
import '../../../../scss/aspian-core/pages/posts/all-posts/_all-posts.scss';
import { connect } from 'react-redux';
import {
  ITaxonomyPost,
  TaxonomyTypeEnum,
} from '../../../../app/models/aspian-core/post';
import { IStoreState } from '../../../../app/stores/reducers';
import { getPosts } from '../../../../app/stores/actions/aspian-core/post/posts';
import { CheckOutlined, CloseOutlined } from '@ant-design/icons';
import moment from 'moment';
import { IPostState } from '../../../../app/stores/reducers/aspian-core/post/posts';
import { WithTranslation, Trans, withTranslation } from 'react-i18next';

interface IProps extends WithTranslation {
  postsState: IPostState;
  getPosts: Function;
}

const PostList: FC<IProps> = ({ postsState, getPosts, t }) => {
  
  const [selectedRowKeys, setSelectedRowKeys] = useState<ReactText[]>([]);

  const onSelectChange = (selectedRowKeys: ReactText[]) => {
    console.log('selectedRowKeys changed: ', selectedRowKeys);
    setSelectedRowKeys(selectedRowKeys);
  };

  const rowSelection: TableRowSelection<object> = {
    selectedRowKeys,
    onChange: onSelectChange,
    selections: [
      Table.SELECTION_ALL,
      Table.SELECTION_INVERT,
      {
        key: 'odd',
        text: 'Select Odd Row',
        onSelect: (changableRowKeys: ReactText[]) => {
          let newSelectedRowKeys = [];
          newSelectedRowKeys = changableRowKeys.filter((key, index) => {
            if (index % 2 !== 0) {
              return false;
            }
            return true;
          });
          setSelectedRowKeys(newSelectedRowKeys);
        },
      },
      {
        key: 'even',
        text: 'Select Even Row',
        onSelect: (changableRowKeys: ReactText[]) => {
          let newSelectedRowKeys = [];
          newSelectedRowKeys = changableRowKeys.filter((key, index) => {
            if (index % 2 !== 0) {
              return true;
            }
            return false;
          });
          setSelectedRowKeys(newSelectedRowKeys);
        },
      },
    ],
  };

  const columns: ColumnType<object>[] = [
    {
      title: <Trans>{t('post-list.table.thead.title')}</Trans>,
      width: 200,
      dataIndex: 'title',
      //fixed: window.innerWidth > 576 ? 'left' : undefined,
      ellipsis: true,
    },
    {
      title: <Trans>{t('post-list.table.thead.category')}</Trans>,
      width: 200,
      dataIndex: 'postCategory',
      ellipsis: true,
    },
    {
      title: <Trans>{t('post-list.table.thead.status')}</Trans>,
      width: 100,
      dataIndex: 'postStatus',
    },
    {
      title: <Trans>{t('post-list.table.thead.attachments')}</Trans>,
      width: 130,
      dataIndex: 'postAttachments',
      align: 'center',
    },
    {
      title: <Trans>{t('post-list.table.thead.comment-allowed')}</Trans>,
      width: 200,
      dataIndex: 'commentAllowed',
      align: 'center',
    },
    {
      title: <Trans>{t('post-list.table.thead.view-count')}</Trans>,
      width: 200,
      dataIndex: 'viewCount',
      align: 'center',
    },
    {
      title: <Trans>{t('post-list.table.thead.pinned')}</Trans>,
      width: 100,
      dataIndex: 'pinned',
      align: 'center',
    },
    {
      title: <Trans>{t('post-list.table.thead.histories')}</Trans>,
      width: 100,
      dataIndex: 'postHistories',
      align: 'center',
    },
    {
      title: <Trans>{t('post-list.table.thead.comments')}</Trans>,
      width: 120,
      dataIndex: 'comments',
      align: 'center',
    },
    {
      title: <Trans>{t('post-list.table.thead.child-posts')}</Trans>,
      width: 150,
      dataIndex: 'childPosts',
      align: 'center',
    },
    {
      title: <Trans>{t('post-list.table.thead.created-at')}</Trans>,
      width: 200,
      dataIndex: 'createdAt',
    },
    {
      title: <Trans>{t('post-list.table.thead.created-by')}</Trans>,
      width: 150,
      dataIndex: 'createdBy',
    },
    {
      title: <Trans>{t('post-list.table.thead.modified-at')}</Trans>,
      width: 150,
      dataIndex: 'modifiedAt',
    },
    {
      title: <Trans>{t('post-list.table.thead.modified-by')}</Trans>,
      width: 150,
      dataIndex: 'modifiedBy',
    },
    {
      title: <Trans>{t('post-list.table.thead.user-agent')}</Trans>,
      width: 150,
      dataIndex: 'userAgent',
      ellipsis: true,
    },
    {
      title: <Trans>{t('post-list.table.thead.ip-address')}</Trans>,
      width: 150,
      dataIndex: 'userIPAddress',
    },
    {
      title: <Trans>{t('post-list.table.thead.actions')}</Trans>,
      key: 'operation',
      //fixed: window.innerWidth > 576 ? 'right' : undefined,
      width: 150,
      align: 'center',
      render: () => (
        <Space>
          <Tooltip
            title={<Trans>{t('post-list.table.tooltip.edit-post')}</Trans>}
            color="gray"
          >
            <Button
              type="link"
              size="middle"
              icon={<EditFilled />}
              className="text warning-color"
            />
          </Tooltip>
          <Tooltip
            title={<Trans>{t('post-list.table.tooltip.delete-post')}</Trans>}
            color="gray"
          >
            <Button type="link" size="middle" icon={<DeleteFilled />} danger />
          </Tooltip>
          <Tooltip
            title={<Trans>{t('post-list.table.tooltip.post-history')}</Trans>}
            color="gray"
          >
            <Button type="link" size="middle" icon={<ClockCircleFilled />} />
          </Tooltip>
        </Space>
      ),
    },
  ];

  let data: object[] = [];

  useEffect(() => {
    getPosts();
  }, [getPosts]);

  postsState.posts.forEach((post, i) => {
    data.push({
      key: i,
      title: post.title,
      postCategory: post.taxonomyPosts.map((taxonomyPost: ITaxonomyPost) =>
        taxonomyPost.taxonomy.type === TaxonomyTypeEnum.category
          ? `${taxonomyPost.taxonomy.term.name} \n`
          : null
      ),
      postStatus: post.postStatus,
      postAttachments: 4,
      commentAllowed: post.commentAllowed ? (
        <CheckOutlined style={{ color: '#52c41a' }} />
      ) : (
        <CloseOutlined style={{ color: '#f5222d' }} />
      ),
      viewCount: post.viewCount,
      pinned: post.isPinned ? (
        <CheckOutlined style={{ color: '#52c41a' }} />
      ) : (
        <CloseOutlined style={{ color: '#f5222d' }} />
      ),
      postHistories: post.postHistories,
      comments: post.comments,
      childPosts: post.childPosts,
      createdAt: moment(post.createdAt).format('YYYY-MM-DD HH:m:s'),
      createdBy: post.createdBy?.userName,
      modifiedAt: post.modifiedAt
        ? moment(post.modifiedAt).format('YYYY-MM-DD HH:m:s')
        : '',
      modifiedBy: post.modifiedBy,
      userAgent: post.userAgent,
      userIPAddress: post.userIPAddress,
    });
  });

  return (
    <Table
      loading={postsState.loadingInitial}
      rowSelection={rowSelection}
      columns={columns}
      dataSource={data}
      size="small"
      scroll={{ x: window.innerWidth - 100, y: window.innerHeight - 100 }}
    />
  );
};

const mapStateToProps = ({
  postsState,
}: IStoreState): { postsState: IPostState } => {
  return { postsState };
};

export default withTranslation()(connect(mapStateToProps, { getPosts })(PostList));
