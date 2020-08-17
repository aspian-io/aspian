import React, {
  useState,
  ReactText,
  FC,
  useEffect,
  Fragment,
  MouseEvent,
} from 'react';
import {
  Table,
  Button,
  Space,
  Tooltip,
  Row,
  Col,
  Typography,
  Popconfirm,
  message,
} from 'antd';
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
import Title from 'antd/lib/typography/Title';
import Paragraph from 'antd/lib/typography/Paragraph';
import Text from 'antd/lib/typography/Text';
import {
  LanguageActionTypeEnum,
  DirectionActionTypeEnum,
} from '../../../../app/stores/actions/aspian-core/locale/types';

interface IProps extends WithTranslation {
  postsState: IPostState;
  getPosts: Function;
  lang: LanguageActionTypeEnum;
  dir: DirectionActionTypeEnum;
}

const PostList: FC<IProps> = ({ postsState, getPosts, t, lang, dir }) => {
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
        text: (
          <Trans>
            {t('post-list.row-selection-menu.items.select-odd-row')}
          </Trans>
        ),
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
        text: (
          <Trans>
            {t('post-list.row-selection-menu.items.select-even-row')}
          </Trans>
        ),
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

  const [windowWidth, setWindowWidth] = useState(window.innerWidth);

  const columns: ColumnType<object>[] = [
    {
      title: <Trans>{t('post-list.table.thead.title')}</Trans>,
      width: 200,
      dataIndex: 'title',
      fixed: windowWidth > 576 ? 'left' : undefined,
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
      fixed: windowWidth > 576 ? 'right' : undefined,
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
    window.addEventListener('resize', (event) => {
      setWindowWidth(window.innerWidth);
    });
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

  function confirm(e: MouseEvent | undefined): void {
    console.log(e);
    message.success('Click on Yes');
  }

  function cancel(e: MouseEvent | undefined): void {
    console.log(e);
    message.error('Click on No');
  }

  return (
    <Fragment>
      <Row align="bottom">
        <Col span={12}>
          <Typography>
            <Title level={4}>
              <Trans>{t('post-list.title')}</Trans>
            </Title>
            <Paragraph ellipsis>
              <Text type="secondary">
                <Trans>{t('post-list.text')}</Trans>
              </Text>
            </Paragraph>
          </Typography>
        </Col>
        <Col
          span={12}
          style={{
            textAlign: dir === DirectionActionTypeEnum.LTR ? 'right' : 'left',
          }}
        >
          <Popconfirm
            title={
              <Trans>{t('post-list.button.delete.popConfirm.title')}</Trans>
            }
            onConfirm={confirm}
            onCancel={cancel}
            okText={
              <Trans>{t('post-list.button.delete.popConfirm.okText')}</Trans>
            }
            cancelText={
              <Trans>
                {t('post-list.button.delete.popConfirm.cancelText')}
              </Trans>
            }
            placement={lang === LanguageActionTypeEnum.en ? 'left' : 'right'}
            okButtonProps={{ danger: true }}
          >
            <Button
              type="primary"
              danger
              icon={<DeleteFilled />}
              size="small"
              style={{ marginBottom: '1rem' }}
            >
              <Trans>{t('post-list.button.delete.name')}</Trans>
            </Button>
          </Popconfirm>
        </Col>
      </Row>
      <Row>
        <Table
          loading={postsState.loadingInitial}
          rowSelection={rowSelection}
          columns={columns}
          dataSource={data}
          size="small"
          scroll={{ x: window.innerWidth - 100, y: window.innerHeight - 100 }}
        />
      </Row>
    </Fragment>
  );
};

const mapStateToProps = ({
  postsState,
  localeState,
}: IStoreState): {
  postsState: IPostState;
  lang: LanguageActionTypeEnum;
  dir: DirectionActionTypeEnum;
} => {
  const { lang, dir } = localeState;
  return { postsState, lang, dir };
};

export default withTranslation()(
  connect(mapStateToProps, { getPosts })(PostList)
);
