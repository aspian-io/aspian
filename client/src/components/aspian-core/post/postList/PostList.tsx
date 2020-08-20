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
  Input,
} from 'antd';
import { TableRowSelection, ColumnsType } from 'antd/lib/table/interface';
import {
  EditFilled,
  DeleteFilled,
  ClockCircleFilled,
  SearchOutlined,
} from '@ant-design/icons';
import '../../../../scss/aspian-core/pages/posts/all-posts/_all-posts.scss';
import { connect } from 'react-redux';
import {
  ITaxonomyPost,
  TaxonomyTypeEnum,
  PostStatusEnum,
} from '../../../../app/models/aspian-core/post';
import { IStoreState } from '../../../../app/stores/reducers';
import {
  getPostsEnvelope,
  setLoading,
} from '../../../../app/stores/actions/aspian-core/post/posts';
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
import { SorterResult, ColumnType } from 'antd/es/table/interface';
import Highlighter from 'react-highlight-words';

interface IProps extends WithTranslation {
  postsState: IPostState;
  getPostsEnvelope: Function;
  lang: LanguageActionTypeEnum;
  dir: DirectionActionTypeEnum;
  setLoading: Function;
}

interface IPostAntdTable {
  key: number;
  title: string;
  postCategory: string[];
  postStatus: PostStatusEnum;
  postAttachments: number;
  commentAllowed: JSX.Element;
  viewCount: number;
  pinned: JSX.Element;
  postHistories: number;
  comments: number;
  childPosts: number;
  createdAt: string;
  createdBy: string;
  modifiedAt: string;
  modifiedBy: string;
  userAgent: string;
  userIPAddress: string;
}

const PostList: FC<IProps> = ({
  postsState,
  getPostsEnvelope,
  t,
  lang,
  dir,
  setLoading,
}) => {
  const [selectedRowKeys, setSelectedRowKeys] = useState<ReactText[]>([]);
  const [searchText, setSearchText] = useState<React.ReactText>('');
  const [searchedColumn, setSearchedColumn] = useState<
    string | number | React.ReactText[] | undefined
  >('');
  const [windowWidth, setWindowWidth] = useState(window.innerWidth);

  const DFAULT_PAGE_SIZE = 10;

  // On select a row event
  const onSelectChange = (selectedRowKeys: ReactText[]) => {
    console.log('selectedRowKeys changed: ', selectedRowKeys);
    setSelectedRowKeys(selectedRowKeys);
  };

  // Row selection functionality implementation
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

  let searchInput: Input;
  // Custom filter functionality implementation
  const getColumnSearchProps = (dataIndex: string): ColumnType<any> => ({
    filterDropdown: ({
      setSelectedKeys,
      selectedKeys,
      confirm,
      clearFilters,
    }) => (
      <div style={{ padding: 8 }}>
        <Input
          ref={(node) => {
            searchInput = node!;
          }}
          placeholder={`Search ${dataIndex}`}
          value={selectedKeys[0]}
          onChange={(e) =>
            setSelectedKeys(e.target.value ? [e.target.value] : [])
          }
          onPressEnter={() => handleSearch(selectedKeys, confirm, dataIndex)}
          style={{ width: 188, marginBottom: 8, display: 'block' }}
        />
        <Space>
          <Button
            type="primary"
            onClick={() => handleSearch(selectedKeys, confirm, dataIndex)}
            icon={<SearchOutlined />}
            size="small"
            style={{ width: 90 }}
          >
            Search
          </Button>
          <Button
            onClick={() => handleReset(clearFilters)}
            size="small"
            style={{ width: 90 }}
          >
            Reset
          </Button>
        </Space>
      </div>
    ),
    filterIcon: (filtered) => (
      <SearchOutlined style={{ color: filtered ? '#1890ff' : undefined }} />
    ),
    // onFilter: (value, record) =>
    //   record[dataIndex]
    //     ? record[dataIndex]
    //         .toString()
    //         .toLowerCase()
    //         .includes(value.toString().toLowerCase())
    //     : '',
    onFilterDropdownVisibleChange: (visible) => {
      if (visible) {
        setTimeout(() => searchInput.select(), 100);
      }
    },
    render: (text: React.ReactText) =>
      searchedColumn === dataIndex ? (
        <Highlighter
          highlightStyle={{ backgroundColor: '#ffc069', padding: 0 }}
          searchWords={[searchText.toString()]}
          autoEscape
          textToHighlight={text ? text.toString().replace(new RegExp(',+$'), '') : ''}
        />
      ) : (
        text
      ),
  });

  const handleSearch = (
    selectedKeys: React.ReactText[],
    confirm: () => void,
    dataIndex: string | number | React.ReactText[] | undefined
  ) => {
    confirm();
    setSearchText(selectedKeys[0]);
    setSearchedColumn(dataIndex);
  };

  const handleReset = (clearFilters: (() => void) | undefined) => {
    clearFilters!();
    setSearchText('');
    setLoading(true) && getPostsEnvelope(DFAULT_PAGE_SIZE, 0);
  };

  const columns: ColumnsType<IPostAntdTable> = [
    {
      title: <Trans>{t('post-list.table.thead.title')}</Trans>,
      width: 200,
      dataIndex: 'title',
      fixed: windowWidth > 576 ? 'left' : undefined,
      ellipsis: true,
      sorter: true,
      ...getColumnSearchProps('title'),
    },
    {
      title: <Trans>{t('post-list.table.thead.category')}</Trans>,
      width: 200,
      dataIndex: 'postCategory',
      ellipsis: true,
      sorter: true,
      ...getColumnSearchProps('postCategory'),
    },
    {
      title: <Trans>{t('post-list.table.thead.status')}</Trans>,
      width: 100,
      dataIndex: 'postStatus',
      sorter: true,
    },
    {
      title: <Trans>{t('post-list.table.thead.attachments')}</Trans>,
      width: 130,
      dataIndex: 'postAttachments',
      align: 'center',
      sorter: true,
    },
    {
      title: <Trans>{t('post-list.table.thead.comment-allowed')}</Trans>,
      width: 200,
      dataIndex: 'commentAllowed',
      align: 'center',
      sorter: true,
    },
    {
      title: <Trans>{t('post-list.table.thead.view-count')}</Trans>,
      width: 200,
      dataIndex: 'viewCount',
      align: 'center',
      sorter: true,
    },
    {
      title: <Trans>{t('post-list.table.thead.pinned')}</Trans>,
      width: 100,
      dataIndex: 'pinned',
      align: 'center',
      sorter: true,
    },
    {
      title: <Trans>{t('post-list.table.thead.histories')}</Trans>,
      width: 100,
      dataIndex: 'postHistories',
      align: 'center',
      sorter: true,
    },
    {
      title: <Trans>{t('post-list.table.thead.comments')}</Trans>,
      width: 120,
      dataIndex: 'comments',
      align: 'center',
      sorter: true,
    },
    {
      title: <Trans>{t('post-list.table.thead.child-posts')}</Trans>,
      width: 150,
      dataIndex: 'childPosts',
      align: 'center',
      sorter: true,
    },
    {
      title: <Trans>{t('post-list.table.thead.created-at')}</Trans>,
      width: 200,
      dataIndex: 'createdAt',
      sorter: true,
    },
    {
      title: <Trans>{t('post-list.table.thead.created-by')}</Trans>,
      width: 150,
      dataIndex: 'createdBy',
      sorter: true,
    },
    {
      title: <Trans>{t('post-list.table.thead.modified-at')}</Trans>,
      width: 150,
      dataIndex: 'modifiedAt',
      sorter: true,
    },
    {
      title: <Trans>{t('post-list.table.thead.modified-by')}</Trans>,
      width: 150,
      dataIndex: 'modifiedBy',
      sorter: true,
    },
    {
      title: <Trans>{t('post-list.table.thead.user-agent')}</Trans>,
      width: 150,
      dataIndex: 'userAgent',
      ellipsis: true,
      sorter: true,
    },
    {
      title: <Trans>{t('post-list.table.thead.ip-address')}</Trans>,
      width: 150,
      dataIndex: 'userIPAddress',
      sorter: true,
      ...getColumnSearchProps('postCategory'),
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

  let data: IPostAntdTable[] = [];

  useEffect(() => {
    getPostsEnvelope(DFAULT_PAGE_SIZE, 0);

    window.addEventListener('resize', (event) => {
      setWindowWidth(window.innerWidth);
    });
  }, [getPostsEnvelope]);

  postsState.postsEnvelope.posts.forEach((post, i) => {
    data.push({
      key: i,
      title: post.title,
      postCategory: post.taxonomyPosts.map((taxonomyPost: ITaxonomyPost) =>
        taxonomyPost.taxonomy.type === TaxonomyTypeEnum.category
          ? `${taxonomyPost.taxonomy.term.name} \n`
          : ''
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
      modifiedBy: post.modifiedBy?.userName,
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
        <Table<IPostAntdTable>
          loading={postsState.loadingInitial}
          rowSelection={rowSelection}
          columns={columns}
          dataSource={data}
          size="small"
          scroll={{ x: window.innerWidth - 100, y: window.innerHeight - 100 }}
          pagination={{
            size: 'small',
            showSizeChanger: true,
            showQuickJumper: true,
            showTotal: (total, range) =>
              `${range[0]}-${range[1]} of ${total} items`,
            total: postsState.postsEnvelope.postCount,
            responsive: true,
          }}
          onChange={(pagination, filters, sorter) => {
            
            const sort = sorter as SorterResult<IPostAntdTable>;

            if (searchInput?.props.value) {
              for (const [key, value] of Object.entries(filters)) {
                if (value) {
                  setLoading(true) &&
                    getPostsEnvelope(
                      pagination.pageSize,
                      pagination.current ? pagination.current! - 1 : undefined,
                      key,
                      value,
                      sort.field,
                      sort.order
                    );
                }
              }
            } else {
              setLoading(true) &&
                getPostsEnvelope(
                  pagination.pageSize,
                  pagination.current ? pagination.current! - 1 : undefined,
                  null,
                  null,
                  sort.field,
                  sort.order
                );
            }
          }}
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
  connect(mapStateToProps, { getPostsEnvelope, setLoading })(PostList)
);
